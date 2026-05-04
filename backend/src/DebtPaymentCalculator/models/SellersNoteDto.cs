namespace DebtPaymentCalculator;

public class SellersNoteDto
{
    public decimal LoanAmount {get; set;}
    public decimal InterestRate {get; set;}
    public int Term {get; set;}

    public void display()
    {
        Console.WriteLine(
            $"{LoanAmount} \n" +
            $"{InterestRate} \n" +
            $"{Term} \n");
    }
}