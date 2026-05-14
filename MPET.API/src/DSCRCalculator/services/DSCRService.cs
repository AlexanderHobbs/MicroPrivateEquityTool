namespace DSCRCalculator;
using Microsoft.Extensions.Caching.Memory;
using Shared.DTOs;

public class DSCRService
{
    public DSCROutputDto Calculate(DSCRDataDto dscrData, DebtOutputDto debtData)
    {
        if (dscrData?.DSCR == null)
            throw new ArgumentNullException(nameof(dscrData), "DSCR data is required.");

        if (debtData == null)
            throw new ArgumentNullException(nameof(debtData), "Debt data is required.");

        decimal profit = 100;
        decimal debtService = debtData.AnnualDebtService;
        DscrDto data = dscrData.DSCR;

        if (debtService == 0)
            throw new InvalidOperationException("Annual debt service cannot be zero.");

        var output = new DSCROutputDto();

        output.DscrRatio = CalculateDSCR(profit, debtService, data, output);
        output.AnnualDebtService = debtService;
        output.AnnualProfit = profit;
        output.WarningLevel = CalculateStatus(output.DscrRatio, data.PreferredDscr);
        output.RemainingCashFlow = CalculateRemainingCashFlow(profit, debtService);

        return output;
    }

    private decimal CalculateDSCR(decimal profit, decimal debtService, DscrDto data, DSCROutputDto output)
    {
        return profit / debtService;
    }

    private DSCROutputDto.Level CalculateStatus(decimal dscrRatio, decimal preferredDscr)
    {
        if (dscrRatio > preferredDscr)
            return DSCROutputDto.Level.Green;

        if (dscrRatio == preferredDscr)
            return DSCROutputDto.Level.Yellow;

        return DSCROutputDto.Level.Red;
    }

    private decimal CalculateRemainingCashFlow(decimal profit, decimal debtService)
    {
        return profit - debtService;
    }

    public DSCROutputDto GetData(DebtOutputDto debtData)
    {
        return new DSCROutputDto
        {
            DscrRatio = 0,
            AnnualDebtService = debtData?.AnnualDebtService ?? 0,
            AnnualProfit = 100, 
            RemainingCashFlow = 0,
            WarningLevel = DSCROutputDto.Level.Red
        };
    }
}