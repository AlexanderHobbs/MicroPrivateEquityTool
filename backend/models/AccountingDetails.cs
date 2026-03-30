namespace EarningsCalculator;

public class AccountingDetail
{
    public int year {get; set;}
    public decimal InterestRate {get; set;}
    public decimal Taxes {get; set;}
    public decimal Depreciation {get; set;}
    public decimal Amortization {get; set;}

    public bool IsAvaliable {get; set;}

    public void displayEntry()
    {   
        if(IsAvaliable)
        {
        Console.WriteLine("EBITDA Details\n");
        Console.WriteLine(
            $"Interest Rate: {InterestRate}\n" +
            $"Taxes: {Taxes}\n" +
            $"Depreciation: {Depreciation}\n" +
            $"Amortization: {Amortization}\n"
        );
        }
        else
        {
            Console.WriteLine("EBITDA Data Not Avalibale\n");
        }

    }
}