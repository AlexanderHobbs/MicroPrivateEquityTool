namespace EarningCalculator;

public class FinancialsDto
{
    public bool IsAvailable {get; set;}
    public decimal InterestRate { get; set; }
    public decimal Taxes { get; set; }
    public decimal Depreciation { get; set; }
    public decimal Amortization { get; set; }
}