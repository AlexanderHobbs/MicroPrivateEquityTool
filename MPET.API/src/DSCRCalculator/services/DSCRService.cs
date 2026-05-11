namespace DSCRCalculator;
using Microsoft.Extensions.Caching.Memory;
using Shared.DTOs;

public class DSCRService
{

    DscrDto data;
    DSCROutputDto output;
    decimal debtService;

    decimal profit = 100000;


    public DSCROutputDto Calculate(DSCRDataDto dscrData, DebtOutputDto debtData)
    {
        data = dscrData.DscrData;
        output = new();
        debtService = debtData.AnnualDebtService;

        calculateDSCR();
        calculateRemainingCashFlow();

        return output;
    }

    public void calculateDSCR()
    {
        output.DscrRatio = profit / debtService;

        calculateStatus(output.DscrRatio);

        output.AnnualDebtService = debtService;
        output.AnnualProfit = profit;
    }

    public void calculateStatus(decimal dscrRatio)
    {
        if(dscrRatio > data.PreferredDscr)
        {
            output.WarningLevel = DSCROutputDto.Level.Green;
        }else if (dscrRatio == data.PreferredDscr)
        {
            output.WarningLevel = DSCROutputDto.Level.Yellow;

        }
        else
        {
            output.WarningLevel = DSCROutputDto.Level.Red;
        }

    }

    public void calculateRemainingCashFlow()
    {
        output.RemainingCashFlow = profit - debtService;
    }

    public DSCROutputDto getData()
    {
        return new DSCROutputDto {
            DscrRatio = 0,
            AnnualDebtService = output.AnnualDebtService,
            AnnualProfit = profit,
            RemainingCashFlow = 0,
            WarningLevel = DSCROutputDto.Level.Red
        };
    }



}