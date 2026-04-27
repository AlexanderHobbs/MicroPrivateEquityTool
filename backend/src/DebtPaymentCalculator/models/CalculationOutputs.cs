namespace DebtPaymentCalculator;

public class CalculationOutput
{
    public decimal AnnualPayment {get; set;}
    public decimal TotalAnnualDebtService {get; set;}
    public List<Amortization> AmortizationSchedule {get; set;} = new();
    public List<decimal> YearlyDebtPayments {get; set;} = new();
    public decimal RemainingBalance {get; set;}
    public decimal TotalInterestPaid {get; set;}
    public decimal AnnualDebtService {get; set;}
    public decimal TotalDebtBurden {get; set;}

    public enum Amortization
    {
        InterestPayment,
        PrinciplePayment,
        EndingBalance,
        BeginningBalance

    };

// Output:
    // Exact yearly debt payments
    // Total interest paid
    // Remaining balance each year
    // Annual debt service 
    // amortization schedule
    // total debt burden
// Ex: “if I buy this for $1.5m, I will owe $x amount per yer”


}