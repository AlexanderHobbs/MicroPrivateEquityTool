namespace EarningsCalculator;

public class Earning
{
    public decimal SDE {get; set;}
    public decimal OwnerSalary {get; set;}

    public void displayEntry()
    {
        Console.WriteLine($"SDE: {SDE:C} \nOwner Salary: {OwnerSalary:C}");
    }

}