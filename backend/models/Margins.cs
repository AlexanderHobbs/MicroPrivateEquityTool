using System.Collections;
namespace EarningsCalculator;

public class Margin
{

    public int year {get; set;}
    public decimal revenue {get; set;}
    public decimal expense {get; set;}

    public void displayEntry()
    {
            Console.WriteLine(
                $"Year: {year}\n" + 
                $"Revenue: {revenue:C}\n" + 
                $"Expense: {expense:C}"
                );
    }


  }