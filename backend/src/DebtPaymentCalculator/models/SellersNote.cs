namespace DebtPaymentCalculator;

public class SellersNoteDto
{
    public decimal Amount {get; set;}
    public decimal InterestRate {get; set;}
    public int Term {get; set;}

    public void display()
    {
        Console.WriteLine(
            $"{Amount} \n" +
            $"{InterestRate} \n" +
            $"{Term} \n");
    }
}