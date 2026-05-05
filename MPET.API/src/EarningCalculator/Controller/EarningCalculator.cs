namespace EarningsCalculator;

public class EarningCalculator
{

    public Dictionary<int, List<object>> InputDictionary;
    public Dictionary<int, CalculationOutput> outputDictionary;
    public EarningCalculator()
    {
        InputDictionary = new();

        outputDictionary = new();
    }

    public void RunProgram()
    {
        AddEntry entry = new(this);
        entry.add();

        Console.WriteLine("Would you like to calculate Financial Summary? (Yes/No)");
        string input = Console.ReadLine().ToUpper();
        int amount = 0;
        if(input.Equals("YES") && amount <= 3){
            Calc calc = new(this);
            calc.calculate();
            amount++;
        }else if (amount == 3)
        {
            Console.WriteLine("Maximum amount of years added");
        }
        
    }
    
}
