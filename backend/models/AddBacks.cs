namespace EarningsCalculator;

public class AddBack 
{

    public int year {get; set;}
    public bool addBacks_present {get; set;}

    public List<AddBackArray> AddBackTotalList {get; set;}

    public class AddBackArray
    {
        public string? description {get; set;}
        public decimal amount {get; set;}
        public decimal categoryWeight {get; set;}

        public enum Category
        {
            non_recurring,
            discretionary,
            questionable
        }

        public Category category {get; set;}

        public int confidenceLevel {get; set;}
    }


    public void displayEntry()
    {
        if(addBacks_present){
            foreach(var singleAb in AddBackTotalList)
            {
                Console.WriteLine($"Add Back Description: {singleAb.description} \n" + 
                                $"Add Back Amount: {singleAb.amount:C} \n" +
                                $"Add Back Category: {singleAb.category}\n" +
                                $"Add Back Confidence Level: {singleAb.confidenceLevel}%\n");
            }
        }
        else
        {
            Console.WriteLine($"No claimed add backs for year: {year}\n");
        }
    }

}