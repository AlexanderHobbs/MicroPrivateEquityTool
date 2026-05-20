namespace EarningCalculator;
using Shared.DTOs;

public class EarningService
{
    public EarningOutputDto Calculate(EarningCalculatorDto eaDto)
    {
        EarningOutputDto output = new();
        decimal prevYearRevenue = 0;

        foreach (var entry in eaDto.InputDictionary.OrderBy(x => x.Key))
        {
            int year = entry.Key;
            var payload = entry.Value;
            EarningOutputDto.EA_Metrics yearData = new() { Year = year };

            // --- Margin / Profit / Revenue ---
            var operating = payload.Operating;
            if (operating != null)
            {
                decimal profit = operating.Revenue - operating.Expense;
                yearData.Profit = profit;

                yearData.Margin = operating.Revenue == 0
                    ? 0
                    : profit / operating.Revenue;

                yearData.RevenueGrowth = prevYearRevenue == 0
                    ? 0
                    : (operating.Revenue - prevYearRevenue) / prevYearRevenue;

                prevYearRevenue = operating.Revenue;

                // --- Add Backs ---
                CalculateAddBacks(payload, yearData);

                // --- EBITDA ---
                yearData.EBITDA = CalculateEBITDA(profit, payload, yearData);

                // --- SDE ---
                CalculateSDE(payload, yearData);

                // --- Adjusted Profit & Margin ---
                yearData.AdjustedProfit = profit + yearData.WeightedAddBacks;

                yearData.AdjustedMargin = operating.Revenue == 0
                    ? 0
                    : yearData.AdjustedProfit / operating.Revenue;
            }

            output.Earning_Output_Dictionary[year] = yearData;
        }

        return output;
    }

    private decimal CalculateEBITDA(decimal profit, PayloadDto payload, EarningOutputDto.EA_Metrics yearData)
    {
        var financials = payload.Financials;

        if (financials != null && financials.IsAvailable)
        {
            return profit
                + financials.InterestRate
                + financials.Taxes
                + financials.Depreciation
                + financials.Amortization;
        }

        // Fallback: EBITDA via SDE method
        return CalculateEBITDA_SDE(payload);
    }

    private decimal CalculateEBITDA_SDE(PayloadDto payload)
    {
        decimal sde = payload.Operating?.ReportedSDE ?? 0;
        decimal ownerSalary = payload.Operating?.OwnerSalary ?? 0;

        decimal nonOperatingAdjustments = payload.Adjustments.AddBacks?
            .Where(ab =>
                ab.category == AdjustmentsDto.AddBackDto.Category.non_recurring ||
                ab.category == AdjustmentsDto.AddBackDto.Category.discretionary)
            .Sum(ab => ab.Amount) ?? 0;

        return sde - ownerSalary + nonOperatingAdjustments;
    }

    private void CalculateAddBacks(PayloadDto payload, EarningOutputDto.EA_Metrics yearData)
    {
        decimal total = 0, weighted = 0, conservative = 0;

        if (payload.Adjustments.AddBacks == null) return;

        foreach (var ab in payload.Adjustments.AddBacks)
        {
            total += ab.Amount;
            weighted += ab.Amount * (ab.ConfidenceLevel / 100m) * ab.CategoryWeight;

            if (ab.category == AdjustmentsDto.AddBackDto.Category.non_recurring && ab.ConfidenceLevel >= 80)
                conservative += ab.Amount;
        }

        yearData.TotalAddBacks = total;
        yearData.WeightedAddBacks = weighted;
        yearData.ConservativeAddBacks = conservative;
        yearData.EffectiveAddBacks = total == 0 ? 0 : weighted / total;
    }

    private void CalculateSDE(PayloadDto payload, EarningOutputDto.EA_Metrics yearData)
    {
        decimal sde = payload.Operating?.ReportedSDE ?? 0;

        yearData.BaseSDE = sde + yearData.TotalAddBacks;
        yearData.RiskAdjustedSDE = sde + yearData.WeightedAddBacks;
        yearData.ConservativeSDE = sde + yearData.ConservativeAddBacks;
    }
}