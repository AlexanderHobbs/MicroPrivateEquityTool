namespace EarningsCalculator;

public class AddBack 
{
    
    
    public string? description {get; set;}
    public decimal amount {get; set;}

    public enum Category
    {
        non_recurring,
        discretionary,
        questionable
    }

    public Category category {get; set;}

    public int confidenceLevel {get; set;}


    public void displayEntry(List<AddBack> ab)
    {
        foreach(var singleAb in ab)
        {
            Console.WriteLine($"Add Back Description: {singleAb.description} \n" + 
                            $"Add Back Amount: {singleAb.amount:C} \n" +
                            $"Add Back Category: {singleAb.category}\n" +
                            $"Add Back Confidence Level: {singleAb.confidenceLevel:P}\n");
        }
    }

}