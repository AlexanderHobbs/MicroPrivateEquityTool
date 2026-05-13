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

        sba = dto.SBA_Metrics;
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

    public void calculateAnnualPayment()
    {
        try{

            if (sba.LoanAmount <= 0)
                throw new ArgumentException("LoanAmount must be > 0");

            if (sba.Term <= 0)
                throw new ArgumentException("Term must be > 0");

            decimal r = sba.InterestRate;
            int n = sba.Term;

            decimal numerator = r * ((decimal) Math.Pow((double) (1 + r), n));
            decimal denominator = (decimal) Math.Pow((double) (1 + r), n) - 1;

            decimal A_AnnualPayment = sba.LoanAmount * (numerator / denominator);
            
            output.SBA_AnnualPayment = A_AnnualPayment;


            r = seller.InterestRate;
            n = seller.Term;

            numerator = r * ((decimal) Math.Pow((double) (1 + r), n));
            denominator = (decimal) Math.Pow((double) (1 + r), n) - 1;

            decimal S_AnnualPaymnet = seller.LoanAmount * (numerator / denominator);
            output.Seller_AnnualPayment = S_AnnualPaymnet;


            output.AnnualDebtService = A_AnnualPayment + S_AnnualPaymnet;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e}");
        }

        /* 
        Annual Payment (A)    
            A = P × [ r(1 + r)^n ] / [ (1 + r)^n − 1 ]

            Where:
            P = loan principal
            r = annual interest rate
            n = total number of years

            Do this separately for:
            SBA loan → A_sba
            Seller note → A_seller

            Then:
            Total Annual Debt Service
            = A_sba + A_seller

            That’s your “what do I owe every year” number.
        */
    
    }

    public void calculateAmortizationSchedule()
    {

        decimal beginningBalance = sba.LoanAmount;

        for(int i = 1; i <= sba.Term; i++)
        {   
            DebtOutputDto.Amortization amortized = new DebtOutputDto.Amortization();

            decimal interest = beginningBalance * sba.InterestRate;

            if (output.SBA_AnnualPayment <= 0)
                throw new Exception("Annual payment not set");

            decimal principal = output.SBA_AnnualPayment - interest;

            decimal endingBalance = beginningBalance - principal;

            amortized.TermYear = i;
            amortized.InterestPayment = interest;
            amortized.PrincipalPayment = principal;
            amortized.EndingBalance = endingBalance;
            amortized.BeginningBalance = beginningBalance;

            output.AmortizationSchedule.Add(amortized);

            beginningBalance = endingBalance;
            
            if(beginningBalance == 0)
            {
                return;
            }

        }

        /* 
        Amortization schedule (year-by-year breakdown).

            For each loan, iterate from Year 1 → n:

            Initialize:

            Beginning Balance₁ = P

            For each year t:

            Interest Payment
            Interestₜ = Beginning Balanceₜ × r
            Principal Payment
            Principalₜ = A − Interestₜ
            Ending Balance
            Ending Balanceₜ = Beginning Balanceₜ − Principalₜ
            Set next year:
            Beginning Balanceₜ₊₁ = Ending Balanceₜ

            Repeat until balance → 0.

            Do this independently for SBA and seller note.
        */

    }

    public void calculateYearlyDebtPayments()
    {
        output.YearlyDebtPayments = new List<DebtOutputDto.YearlyDebt>();

        decimal sbaPayment = output.SBA_AnnualPayment;
        decimal sellerPayment = output.Seller_AnnualPayment;

        int maxYears = Math.Max(sba.Term, seller.Term);

        for(int i = 1; i <= maxYears; i++)
        {
            output.YearlyDebtPayments.Add(new DebtOutputDto.YearlyDebt
            {
                Year = i,
                SBA_Payment = 1 <= sba.Term ? sbaPayment : 0,
                Seller_Payment = 1 <= seller.Term ? sellerPayment : 0,
                TotalPayment = (i <= sba.Term ? sbaPayment : 0) + (i <= seller.Term ? sellerPayment : 0)
            });
        }
        
        /*
        Exact Yearly Debt Payments
        For each year:
        Total Paymentₜ = A_sba + A_seller
        (If terms differ, seller note may drop off earlier—handle that by setting its payment to 0 after maturity.)
        */
    
    }

    public void calculateRemainingBalance()
    {
        output.RemainingBalances = new List<DebtOutputDto.RemainingBalance>();

        decimal sbaBalance = sba.LoanAmount;
        decimal sellerBalance = seller.LoanAmount;

        foreach(var year in output.YearlyDebtPayments)
        {
            sbaBalance = Math.Max(0, sbaBalance - year.SBA_Payment);
            sellerBalance = Math.Max(0, sellerBalance - year.Seller_Payment);

            output.RemainingBalances.Add(new DebtOutputDto.RemainingBalance
            {
                Year = year.Year,
                SBA_Balance = sbaBalance,
                Seller_Balance = sellerBalance,
                TotalBalance = sbaBalance + sellerBalance
            });
        }

        /*
            Remaining Balance Each Year
            Remaining Balanceₜ = SBA Balanceₜ + Seller Balanceₜ
        */
    
    }

    public void calculateTotalInterestPaid()
    {

        decimal sbaInterest = 0;
        decimal sellerInterest = 0;

        foreach(var year in output.AmortizationSchedule)
        {
            sbaInterest += year.InterestPayment;
        }

        sellerInterest = (output.Seller_AnnualPayment * seller.Term) - seller.LoanAmount;

        output.TotalInterestPaid = sbaInterest + sellerInterest;

        /*
            Total Interest Paid

            For each loan:
            Total Interest = (Annual Payment × n) − Principal

            Then:

            Total Interest Paid (combined)
            = Interest_sba + Interest_seller
        */
    
    }
    
    public void calculateAnnualDebtService()
    {
        output.AnnualDebtService = output.SBA_AnnualPayment + output.Seller_AnnualPayment;

        /*
            Annual Debt Service

            This is just:

            Constant if fully amortized loans → A_sba + A_seller
            Variable if terms differ → sum of active loan payments per year

            You may want:

            Year 1 debt service
            Average debt service
            Max debt service (if structures differ)
        */
    
    }

    public void calculateTotalDebtBurden()
    {

        decimal sbaTotal = output.SBA_AnnualPayment * sba.Term;

        decimal sellerTotal = output.Seller_AnnualPayment * seller.Term;

        output.TotalDebtBurden = sbaTotal * sellerTotal;

        /*
            Total Debt Burden

            This is the full cost of borrowing:

            Total Debt Burden =
            Total Principal Borrowed + Total Interest Paid

            Or equivalently:

            = (A_sba × n_sba) + (A_seller × n_seller)

            This tells you the total cash outflow required to fully service the debt.   
        */
        
    }

}

/*
    // Amortization Schedule (final structure)

    // For each year, your combined table should look like:

    // Year
    // Beginning Balance (combined)
    // Total Payment
    // Interest Paid (combined)
    // Principal Paid (combined)
    // Ending Balance (combined)

    // Optionally include SBA vs Seller columns for transparency.

    // If you want this to reflect real SBA deals more accurately, you’ll eventually need to support:

    // Monthly compounding (then aggregate to yearly)
    // Interest-only periods (common in seller notes)
    // Balloon payments (less common but possible)

    // But for a clean, decision-grade calculator, annual amortization as described above is sufficient and far more interpretable.
        
*/