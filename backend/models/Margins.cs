using System.Collections;
namespace EarningsCalculator;

public class Margin
{

    //public static Dictionary<int, float> revenueDictionary {get; set;} = new();
    //public static Dictionary<int, float> ExpenseDictionary {get; set;} = new();
    //public static Dictionary<int, AccountingDetail> ExpenseDictionary {get; set;} = new();
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

    // public void displayEntryDictionary(Dictionary<int, Margin> mg)
    // {
    //      foreach(KeyValuePair<int, Margin> entry in mg)
    //     {
    //         Console.WriteLine($"Year: {mg}\n" 
    //                         + $"Revenue: {mg}\n" 
    //                         + $"Expense: {mg}");
    //     }
    // }

  }