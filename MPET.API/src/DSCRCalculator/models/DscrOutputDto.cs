namespace DSCRCalculator;

public class DSCROutputDto
{
    public decimal DscrRatio {get; set;}
    public decimal RemainingCashFlow {get; set;}
    public Level WarningLevel {get; set;}
    public enum Level
    {
        Green,
        Yellow,
        Red
    } 

    public decimal AnnualDebtService {get; set;}
    public decimal AnnualProfit {get; set;}
}