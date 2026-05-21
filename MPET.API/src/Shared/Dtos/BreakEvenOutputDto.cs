namespace Shared.DTOs;

public class BreakEvenOutputDto
{
    public decimal BreakEvenRevenue {get; set;}
    public decimal DropTolerance {get; set;}
    public decimal Cushion {get; set;}

    // BER = (Fixed Costs + Debt Service) / (1 - Variable Cost %)

// Drop Tolerance = ((Current Revenue - BER) / Current Revenue) × 100

// Cushion = Current Revenue - BER
}