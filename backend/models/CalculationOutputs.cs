namespace EarningsCalculator;

public class CalculationOutput
{
    public Dictionary<int,decimal>? Profit_Per_Year {get; set;} = new();
    public Dictionary<int, decimal>? Margin_Per_Year {get; set;} = new();
    public Dictionary<int, decimal>? Revenue_Growth {get; set;} = new();
    public Dictionary<int, decimal>? EBITDA {get; set;} = new();
    public decimal Effective_AddBacks {get; set;}
    public decimal Total_AddBacks {get; set;}
    public decimal Weighted_AddBacks {get; set;}
    public decimal Base_SDE {get; set;}
    public decimal Risk_Adjusted_SDE {get; set;}
    public decimal Conservative_SDE {get; set;}
    public decimal Yearly_Adjusted_Profit {get; set;}
    public decimal Yearly_Adjusted_Margin {get; set;}

}