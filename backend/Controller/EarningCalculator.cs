namespace EarningsCalculator;

public class EarningCalculator
{

    public Dictionary<int, List<object>> InputDictionary = new();
    public Dictionary<int, List<object>> outputDictionary = new();
    public List<object> EntryList {get; set;}
    public List<object> OutputList {get; set;}
    public EarningCalculator()
    {
        EntryList = new List<object>();
        OutputList = new List<object>();
    }

    public void runProgram()
    {
        AddEntry entry = new(this);
        entry.add();

        Calc calc = new(this);
        calc.calculate();
        
    }
    
}
