namespace BreakEvenCalculator;
using Shared.DTOs;

public class BreakEvenService
{
    public BreakEvenOutputDto Calculate(BreakEvenDto eaDto)
    {
        BreakEvenOutputDto output = new();

        decimal debtService = eaDto.BreakEvenModel.DebtService;
        decimal currentRevenue = eaDto.BreakEvenModel.CurrentRevenue;
        decimal fixedCost = eaDto.BreakEvenModel.FixedCost;
        decimal variableCost = eaDto.BreakEvenModel.VariableCost;

        decimal breakEvenRevenue = CalculateBER(debtService, fixedCost, variableCost);
        decimal dropTolerance = CalculateDT(currentRevenue, breakEvenRevenue);
        decimal cushion = CalculateCushion(currentRevenue, breakEvenRevenue);

        output.BreakEvenRevenue = breakEvenRevenue;
        output.DropTolerance = dropTolerance;
        output.Cushion = cushion;

        return output;
    }

    public decimal CalculateBER(decimal debtService, decimal FixedCost, decimal VariableCost)
    {
        return (FixedCost + debtService) / (1 - VariableCost);
    }

    public decimal CalculateDT(decimal currentRev, decimal BER)
    {
        return ((currentRev - BER) / currentRev) * 100;
    }

    public decimal CalculateCushion(decimal currentRev, decimal BER)
    {
        return currentRev - BER;
    }

}