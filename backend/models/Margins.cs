using System.Collections;
namespace EarningsCalculator;

public class Margin
{

    public int year {get; set;}
    public decimal revenue {get; set;}
    public decimal expense {get; set;}

    public void displayEntry(List<Margin> mg)
    {
        foreach(var singleMargin in mg)
        {
            Console.WriteLine($"Year: {singleMargin.year}\n" 
                            + $"Revenue: {singleMargin.revenue:C}\n" 
                            + $"Expense: {singleMargin.expense:C}");
        }
    }


  }