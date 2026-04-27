
using EarningsCalculator;
using DebtPaymentCalculator;

namespace AcquisitionInt;
public class financialSummary
{
    public static void Main()
    {
       EarningCalculator compiler = new();
       DebtCalculator compiler2 = new();
       compiler.RunProgram();
    //    compiler2.RunProgram();

    }
    
}
