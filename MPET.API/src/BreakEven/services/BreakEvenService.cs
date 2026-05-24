namespace BreakEvenCalculator;
using Shared.DTOs;

public class BreakEvenService
{
    public BreakEvenOutputDto Calculate(BreakEvenDto eaDto)
    {
        try {
            BreakEvenOutputDto output = new();

            decimal debtService = eaDto.BreakEvenModel.DebtService;
            decimal currentRevenue = eaDto.BreakEvenModel.CurrentRevenue;
            decimal fixedCost = eaDto.BreakEvenModel.FixedCost;
            decimal variableCost = eaDto.BreakEvenModel.VariableCost;

            decimal variableRatio = CalculateVariableRatio(variableCost, currentRevenue);

            decimal breakEvenRevenue = CalculateBER(debtService, fixedCost, variableRatio);
            decimal cushion = CalculateCushion(currentRevenue, breakEvenRevenue);
            decimal dropTolerance = CalculateDT(currentRevenue, cushion);


            output.BreakEvenRevenue = breakEvenRevenue;
            output.DropTolerance = dropTolerance;
            output.Cushion = cushion;

            return output;

        }catch (Exception e)
        {
            Console.WriteLine($"Error in calculations: {e}");
            throw;
        }
    }

    public decimal CalculateVariableRatio(decimal cost, decimal revenue)
    {
        if(revenue == 0)
        {
            throw new ArgumentException("Revenue cannot be zero");
        }
        
        return cost / revenue;
    }

    public decimal CalculateBER(decimal debtService, decimal fixedCost, decimal variableRatio)
    {
        decimal contributionMargin = 1 - variableRatio;
        Console.WriteLine(fixedCost + "-" + debtService +"-"+ contributionMargin);

        return (fixedCost + debtService) / contributionMargin;
    }

    public decimal CalculateDT(decimal currentRev, decimal cushion)
    {
        if(currentRev == 0)
        {
            throw new ArgumentException("Revenue cannot be zero");
        }

        return cushion / currentRev * 100;
    }

    public decimal CalculateCushion(decimal currentRev, decimal BER)
    {
        if(currentRev == 0)
        {
            throw new ArgumentException("Revenue cannot be zero");
        }

        return currentRev - BER;
    }

}