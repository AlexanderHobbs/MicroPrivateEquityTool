namespace DebtPaymentCalculator;

public class Calculate
{

    CalculationOutput output = new CalculationOutput();
    public void calculateAnnualPayment()
    {

        SBAMetricsDto sba = new SBAMetricsDto();

        decimal r = sba.InterestRate;
        int n = sba.Term;

        decimal numerator = r * ((decimal) Math.Pow((double) (1 + r), n));
        decimal denominator = (decimal) Math.Pow((double) (1 + r), n) - 1;

        decimal A_AnnualPayment = sba.LoanAmount * (numerator / denominator);
        
        output.AnnualPayment = A_AnnualPayment;



        SellersNoteDto s_note = new SellersNoteDto();

        r = s_note.InterestRate;
        n = s_note.Term;

        numerator = r * ((decimal) Math.Pow((double) (1 + r), n));
        denominator = (decimal) Math.Pow((double) (1 + r), n) - 1;

        decimal S_AnnualPaymnet = s_note.LoanAmount * (numerator / denominator);



        output.AnnualDebtService = A_AnnualPayment + S_AnnualPaymnet;


        /* Annual Payment (A)    
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

    public void calculateAmortizationZchedule()
    {
        /* Amortization schedule (year-by-year breakdown).

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
    
    //     Exact Yearly Debt Payments
    //     For each year:
    //     Total Paymentₜ = A_sba + A_seller
    //     (If terms differ, seller note may drop off earlier—handle that by setting its payment to 0 after maturity.)
    
    }

    public void calculateRemainingBalance()
    {
        // Remaining Balance Each Year
        // Remaining Balanceₜ = SBA Balanceₜ + Seller Balanceₜ
    }

    public void calculateTotalInterestPaid()
    {
        // Total Interest Paid

        // For each loan:
        // Total Interest = (Annual Payment × n) − Principal

        // Then:

        // Total Interest Paid (combined)
        // = Interest_sba + Interest_seller
    }
    
    public void calculateAnnualDebtService()
    {
        // Annual Debt Service

        // This is just:

        // Constant if fully amortized loans → A_sba + A_seller
        // Variable if terms differ → sum of active loan payments per year

        // You may want:

        // Year 1 debt service
        // Average debt service
        // Max debt service (if structures differ)
    }

    public void calculateTotalDebtBurden()
    {
        // Total Debt Burden

        // This is the full cost of borrowing:

        // Total Debt Burden =
        // Total Principal Borrowed + Total Interest Paid

        // Or equivalently:

        // = (A_sba × n_sba) + (A_seller × n_seller)

        // This tells you the total cash outflow required to fully service the debt.   
        
    }

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
        
}