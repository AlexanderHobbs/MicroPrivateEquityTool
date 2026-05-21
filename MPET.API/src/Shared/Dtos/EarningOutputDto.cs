namespace Shared.DTOs;

public class EarningOutputDto
{
    public Dictionary<int, EA_Metrics> Earning_Output_Dictionary { get; set; } = new();


    public class EA_Metrics
    {
        public int Year { get; set; }

        // Core
        public decimal Profit { get; set; }
        public decimal Margin { get; set; }
        public decimal RevenueGrowth { get; set; }
        public decimal EBITDA { get; set; }
        public decimal Variance {get; set;}

        // Add Backs
        public decimal TotalAddBacks { get; set; }
        public decimal WeightedAddBacks { get; set; }
        public decimal ConservativeAddBacks { get; set; }
        public decimal EffectiveAddBacks { get; set; }

        // SDE
        public decimal BaseSDE { get; set; }
        public decimal RiskAdjustedSDE { get; set; }
        public decimal ConservativeSDE { get; set; }

        // Adjusted
        public decimal AdjustedProfit { get; set; }
        public decimal AdjustedMargin { get; set; }
    }
}