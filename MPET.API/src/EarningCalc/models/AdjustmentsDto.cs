namespace EarningCalculator;

public class AdjustmentsDto
{
    public List<AddBackDto> AddBacks { get; set; } = new();

    public class AddBackDto
    {
        public string? Description {get; set;}
        public decimal Amount {get; set;}
        public decimal CategoryWeight {get; set;}

        public enum Category
        {
            non_recurring,
            discretionary,
            questionable
        }

        public Category category {get; set;}

        public int ConfidenceLevel {get; set;}
    }
}