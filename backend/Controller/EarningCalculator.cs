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
        if(Console.ReadLine().Equals("Yes")){
            Calc calc = new(this);
            calc.calculate();
        }
        
    }
    
}
