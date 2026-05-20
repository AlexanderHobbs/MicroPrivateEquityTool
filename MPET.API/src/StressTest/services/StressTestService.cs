using Shared.DTOs;

namespace StressTestCalculator;

public class StressTestService
{

    public StressTestOutputDto calculate(StressTestModel stModel, DebtOutputDto debtInput)
    {

        
        StressTestOutputDto output = new();

        decimal AdjustedProfit = UpdateAdjustedProfit(stModel);
        decimal AdjustedDSCR = UpdateDSCR(AdjustedProfit, debtInput.AnnualDebtService);
        decimal CashFlow = UpdateCashFlow(AdjustedProfit, debtInput.AnnualDebtService);
        string GeneratedSuggestion = UpdateSuggestion(AdjustedDSCR, CashFlow, stModel.NerveLevel);

        output.AdjustedProfit = AdjustedProfit;
        output.AdjustedDSCR = AdjustedDSCR;
        output.CashFlow = CashFlow;
        output.GeneratedSuggestion = GeneratedSuggestion;


        return output;
    }

    public decimal UpdateAdjustedProfit(StressTestModel stModel)
    {
        decimal adjustedRevenue = stModel.Revenue * (1 - stModel.RevenueDrop / 100);
        decimal adjustedExpense = stModel.Expense * (1 - stModel.MarginLevelShift / 100);
        return adjustedRevenue - adjustedExpense;
    }

    public decimal UpdateDSCR(decimal adjustedProfit, decimal annualDebtService)
    {
        if(annualDebtService == 0) return 0;
        return adjustedProfit / annualDebtService;
    }

    public decimal UpdateCashFlow(decimal adjustedProfit, decimal annualDebtService)
    {
        return adjustedProfit - annualDebtService;
    }

    public string UpdateSuggestion(decimal adjustedDSCR, decimal CashFlow, decimal nerveLevel)
    {
        if (adjustedDSCR >= 1.25m && CashFlow > 0)
            return "Deal remains viable under stress conditions. Comfortable margin above debt obligations.";

        if (adjustedDSCR >= 1.0m && CashFlow > 0)
            return "Deal is survivable but tight. Consider reducing expenses or renegotiating debt terms.";

        if (adjustedDSCR < 1.0m && nerveLevel >= 70)
            return "High risk. Revenue drop pushes DSCR below 1.0. Deal cannot cover debt — proceed with caution or walk away.";

        if (adjustedDSCR < 1.0m && nerveLevel < 70)
            return "Deal breaks under these stress conditions. DSCR below 1.0 means debt cannot be covered. Not recommended.";

        return "Insufficient data to generate a suggestion.";
    }
}