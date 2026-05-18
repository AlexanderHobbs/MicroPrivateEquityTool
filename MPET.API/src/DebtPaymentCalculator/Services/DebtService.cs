namespace DebtPaymentCalculator;
using Shared.DTOs;


public class DebtService
{

    SBAMetricsDto sba;
    SellersNoteDto seller;
    PurchaseDto purchase;
    DebtOutputDto output;


    public DebtOutputDto Calculate(DebtDataDto dto)
    {
        output = new DebtOutputDto();

        sba    = dto.SBA_Metrics;
        seller = dto.SellersNote;
        purchase = dto.Purchase;

        calculateAnnualPayment();
        calculateAmortizationSchedule();
        calculateYearlyDebtPayments();
        calculateRemainingBalance();
        calculateTotalInterestPaid();
        calculateTotalDebtBurden();

        return output;
    }


    // ── Annual payment ────────────────────────────────────────────────────────
    // Formula: A = P × [ r(1+r)^n ] / [ (1+r)^n − 1 ]
    // Rates are stored as whole numbers (e.g. 11.25) and divided by 100 here.
    // ─────────────────────────────────────────────────────────────────────────
    public void calculateAnnualPayment()
    {
        try
        {
            if (sba.LoanAmount <= 0)
                throw new ArgumentException("LoanAmount must be > 0");
            if (sba.Term <= 0)
                throw new ArgumentException("Term must be > 0");

            // FIX: convert whole-number % → decimal rate before applying formula
            decimal r = sba.InterestRate / 100m;
            int     n = sba.Term;

            decimal pow         = (decimal)Math.Pow((double)(1 + r), n);
            decimal numerator   = r * pow;
            decimal denominator = pow - 1;

            output.SBA_AnnualPayment = sba.LoanAmount * (numerator / denominator);

            // Seller's note
            r = seller.InterestRate / 100m;
            n = seller.Term;

            pow         = (decimal)Math.Pow((double)(1 + r), n);
            numerator   = r * pow;
            denominator = pow - 1;

            output.Seller_AnnualPayment = seller.LoanAmount * (numerator / denominator);

            output.AnnualDebtService = output.SBA_AnnualPayment + output.Seller_AnnualPayment;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in calculateAnnualPayment: {e}");
            throw;
        }
    }


    // ── Amortization schedule (SBA loan only) ─────────────────────────────────
    // Year-by-year: Interest = beginBalance × r, Principal = payment − interest
    // ─────────────────────────────────────────────────────────────────────────
    public void calculateAmortizationSchedule()
    {
        if (output.SBA_AnnualPayment <= 0)
            throw new Exception("Annual payment not set — call calculateAnnualPayment first.");

        decimal r               = sba.InterestRate / 100m;   // FIX: decimal rate
        decimal beginningBalance = sba.LoanAmount;

        for (int i = 1; i <= sba.Term; i++)
        {
            decimal interest        = beginningBalance * r;
            decimal principal       = output.SBA_AnnualPayment - interest;
            decimal endingBalance   = beginningBalance - principal;

            output.AmortizationSchedule.Add(new DebtOutputDto.Amortization
            {
                TermYear          = i,
                BeginningBalance  = beginningBalance,
                InterestPayment   = interest,
                PrincipalPayment  = principal,
                EndingBalance     = Math.Max(0, endingBalance)   // guard float drift on final year
            });

            beginningBalance = Math.Max(0, endingBalance);
            if (beginningBalance == 0) return;
        }
    }


    // ── Yearly combined payments ───────────────────────────────────────────────
    // FIX: loop variable i must be compared to term limits, not the constant 1
    // ─────────────────────────────────────────────────────────────────────────
    public void calculateYearlyDebtPayments()
    {
        output.YearlyDebtPayments = new List<DebtOutputDto.YearlyDebt>();

        int maxYears = Math.Max(sba.Term, seller.Term);

        for (int i = 1; i <= maxYears; i++)
        {
            decimal sbaPayment    = i <= sba.Term    ? output.SBA_AnnualPayment    : 0;   // FIX: was "1 <="
            decimal sellerPayment = i <= seller.Term ? output.Seller_AnnualPayment : 0;   // FIX: was "1 <="

            output.YearlyDebtPayments.Add(new DebtOutputDto.YearlyDebt
            {
                Year          = i,
                SBA_Payment    = sbaPayment,
                Seller_Payment = sellerPayment,
                TotalPayment   = sbaPayment + sellerPayment
            });
        }
    }


    // ── Remaining balance each year ───────────────────────────────────────────
    // FIX: subtract only the principal portion, not the full payment.
    //      Full payment includes interest, so subtracting it undershoots the
    //      balance and produces a negative balance long before maturity.
    // ─────────────────────────────────────────────────────────────────────────
    public void calculateRemainingBalance()
    {
        output.RemainingBalances = new List<DebtOutputDto.RemainingBalance>();

        decimal sbaBalance    = sba.LoanAmount;
        decimal sellerBalance = seller.LoanAmount;

        decimal sbaRate    = sba.InterestRate    / 100m;
        decimal sellerRate = seller.InterestRate / 100m;

        foreach (var year in output.YearlyDebtPayments)
        {
            // SBA: derive principal from amortization schedule when available
            if (year.Year <= sba.Term)
            {
                decimal sbaInterest  = sbaBalance * sbaRate;
                decimal sbaPrincipal = year.SBA_Payment - sbaInterest;
                sbaBalance = Math.Max(0, sbaBalance - sbaPrincipal);
            }

            // Seller's note: compute principal inline (same formula)
            if (year.Year <= seller.Term)
            {
                decimal sellerInterest  = sellerBalance * sellerRate;
                decimal sellerPrincipal = year.Seller_Payment - sellerInterest;
                sellerBalance = Math.Max(0, sellerBalance - sellerPrincipal);
            }

            output.RemainingBalances.Add(new DebtOutputDto.RemainingBalance
            {
                Year           = year.Year,
                SBA_Balance    = sbaBalance,
                Seller_Balance = sellerBalance,
                TotalBalance   = sbaBalance + sellerBalance
            });
        }
    }


    // ── Total interest paid ───────────────────────────────────────────────────
    // SBA:    sum interest column from amortization schedule (exact)
    // Seller: (payment × term) − principal  (no per-year schedule needed)
    // ─────────────────────────────────────────────────────────────────────────
    public void calculateTotalInterestPaid()
    {
        decimal sbaInterest = 0;
        foreach (var year in output.AmortizationSchedule)
            sbaInterest += year.InterestPayment;

        decimal sellerInterest = (output.Seller_AnnualPayment * seller.Term) - seller.LoanAmount;

        output.TotalInterestPaid = sbaInterest + sellerInterest;
    }


    // ── Total debt burden ─────────────────────────────────────────────────────
    // Total cash out = (SBA payment × SBA term) + (seller payment × seller term)
    // FIX: was multiplying the two totals instead of adding them
    // ─────────────────────────────────────────────────────────────────────────
    public void calculateTotalDebtBurden()
    {
        decimal sbaTotal    = output.SBA_AnnualPayment    * sba.Term;
        decimal sellerTotal = output.Seller_AnnualPayment * seller.Term;

        output.TotalDebtBurden = sbaTotal + sellerTotal;   // FIX: was sbaTotal * sellerTotal
    }
}

/*
    Notes for future enhancements (from original comments):

    Monthly compounding — divide rate by 12, multiply term by 12, then aggregate
    to yearly totals for reporting. More accurate for SBA 7(a) which compounds monthly.

    Interest-only periods — common in seller notes; zero principal for N years,
    then switch to amortizing formula for remainder of term.

    Balloon payments — store residual balance at a given year and add to final payment.
*/