namespace EarningsCalculator;

public class CalculationOutput
{
    public int year {get; set;}
    public decimal Profit_Per_Year {get; set;}
    public decimal Margin_Per_Year {get; set;}
    public decimal Revenue_Growth {get; set;}
    public decimal EBITDA {get; set;}
    public decimal Effective_AddBacks {get; set;}
    public decimal Total_AddBacks {get; set;}
    public decimal Weighted_AddBacks {get; set;}
    public decimal Conservative_AddBacks {get; set;}
    public decimal Base_SDE {get; set;}
    public decimal Risk_Adjusted_SDE {get; set;}
    public decimal Conservative_SDE {get; set;}
    public decimal Yearly_Adjusted_Profit {get; set;}
    public decimal Yearly_Adjusted_Margin {get; set;}

    public void displayOutput()
    {
        Console.WriteLine(
            $"Year: {year}\n" +
            $"Profit Per Year: {Profit_Per_Year:C}\n" +
            $"Margin Per Year: {Margin_Per_Year:P1}\n");
            
        if(Revenue_Growth == 0)
        {
            Console.WriteLine($"Revenue Growth: NA (Previous Year Data Not Avaliable)\n");
        }
        else
        {
            Console.WriteLine($"Revenue Growth: {Revenue_Growth: P1}");
        }

        Console.WriteLine(
            $"EBITDA: {EBITDA:C}\n" +
            $"Add Back Effectiveness: {Effective_AddBacks:P1}\n" +
            $"Total Add Backs: {Total_AddBacks:C}\n" +
            $"Weighted Add Backs: {Weighted_AddBacks:C}\n" +
            $"Conservative Add Backs: {Conservative_AddBacks:C}\n" +
            $"Base SDE: {Base_SDE:C}\n" +
            $"Risk Adjusted SDE: {Risk_Adjusted_SDE:C}\n" +
            $"Conservative SDE: {Conservative_SDE:C}\n" +
            $"Yearly Adjusted Profit: {Yearly_Adjusted_Profit:C}\n" +
            $"Yearly Adjusted Margin: {Yearly_Adjusted_Margin:P1}"
        );
    }

}