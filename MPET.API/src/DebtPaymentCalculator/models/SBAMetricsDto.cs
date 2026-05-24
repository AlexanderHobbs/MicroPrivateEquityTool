namespace DebtPaymentCalculator;

public class SBAMetricsDto
{
    public decimal LoanAmount {get; set;}
    public decimal DownPayment {get; set;}
    public bool IsAutocalculated {get; set;}
    public decimal InterestRate {get; set;}
    public int Term {get; set;}

}