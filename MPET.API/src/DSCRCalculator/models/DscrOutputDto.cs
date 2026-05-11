namespace DSCRCalculator;

public class DSCROutputDto
{
    public decimal DscrRatio {get; set;} = 0;
    public decimal RemainingCashFlow {get; set;} = 0;
    public Level WarningLevel {get; set;}
    public enum Level
    {
        Green,
        Yellow,
        Red
    } 

    public decimal AnnualDebtService {get; set;} = 0;
    public decimal AnnualProfit {get; set;} = 0;
}