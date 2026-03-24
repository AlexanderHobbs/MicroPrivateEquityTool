using Microsoft.VisualBasic;

namespace EarningsCalculator;

public class Calculations : EarningCalculator
{
    CalculationOutput output;
    private readonly EarningCalculator _calculator;
    public Calculations(EarningCalculator calculator)
    {
        _calculator = calculator;
        output = new();
    }
    public void calculate()
    {
        // calculateMarginTrend();
        // calculateRevenueTrend();
        calculateFinancialSummary();
        // calculateSDE();
    }

    public void calculateFinancialSummary()
    {
        try{ 

            decimal profit, SDE, margin, Revenue_growth, EBIDTA;

            foreach (object entry in _calculator.EntryList){
            
                if(entry is List<Margin> list)
                {   
                    int index = 0;
                    foreach (Margin mg in list){
                    {
                        profit = mg.revenue - mg.expense;
                        output.Profit_Per_Year.Add(mg.year, profit);
                    }

                    {
                        margin = profit / mg.revenue;
                        output.Margin_Per_Year.Add(mg.year, margin);
                    }

                    {
                        if(index-1 >= 0){
                            Margin prevYear = list[index-1];
                            Revenue_growth = (mg.revenue - prevYear.revenue) / prevYear.revenue;
                        }
                        else
                        {
                            Revenue_growth = -1;
                        }

                        index++;
                        output.Revenue_Growth.Add(mg.year, Revenue_growth);
                    }

                    {
                        EBIDTA = calculateEBITDA(profit);
                        output.EBITDA.Add(mg.year, EBIDTA);
                        

                    }

                    {
                        calculateAddBacks();
                    }

                    {
                       calculateSDE(); 
                    }

                    {
                        
                    }
                    
                    
                    

                    Console.WriteLine($"{profit:C}, {margin:P1}, {(Revenue_growth == -1 ? "unavaliable" : Revenue_growth)}, {EBIDTA:C}");
                    
                    }
                }
            }
        }catch (Exception e)
        {
            Console.WriteLine($"Exception: {e}");
        }
    }

    // public double calculateRevenueGrowth(Margin mg)
    // {
        
    // }
    public void calculateMarginTrend()
    {
    
    }

    public void calculateRevenueTrend()
    {
        
    }

    public decimal calculateEBITDA(decimal profit)
    {
        decimal EBITDA_VALUE = 0;
        decimal SDE = 0, OwnerSalary = 0, non_operating_adjustments = 0;

        foreach (object entry in _calculator.EntryList){
            
            if(entry is AccountingDetail list)
            {
                switch (list.avaliableData)
                {
                    case AccountingDetail.Avaliable.avaliable:
                        Console.WriteLine("avaliable");
                        EBITDA_VALUE =  profit + list.InterestRate + list.Taxes + list.Depreciation + list.Amortization;
                        break;

                    case AccountingDetail.Avaliable.not_avaliable:
                        EBITDA_VALUE = calculateEbitdaSDE();
                        break;
                }
            }
        }

        return EBITDA_VALUE;

    }

    public decimal calculateEbitdaSDE()
    {
        decimal SDE = 0, OwnerSalary = 0, non_operating_adjustments = 0;

        foreach (object entry in _calculator.EntryList){
            if(entry is Earning earning)
            {
                SDE = earning.SDE;
                Console.WriteLine(SDE);
                OwnerSalary = earning.OwnerSalary;
            }else if (entry is List<AddBack> addBack)
            {
                foreach(AddBack ab in addBack)
                {
                    non_operating_adjustments += ab.amount;
                }
            }
        }



        return SDE - OwnerSalary + non_operating_adjustments;
    }

    public void calculateAddBacks()
    {
        decimal Effective_AddBack = 0, Total_AddBacks = 0, Weighted_AddBacks = 0;
        foreach (object entry in _calculator.EntryList){
            
            if (entry is List<AddBack> addBack)
            {
                foreach(AddBack ab in addBack)
                {
                    switch (ab.category)
                    {
                        case AddBack.Category.non_recurring:
                            Effective_AddBack = ab.amount * (ab.confidenceLevel / 100) * (decimal) 1.0;
                        break;

                        case AddBack.Category.discretionary:
                            Effective_AddBack = ab.amount * (ab.confidenceLevel / 100) * (decimal) .75;
                        break;

                        case AddBack.Category.questionable:
                            Effective_AddBack = ab.amount * (ab.confidenceLevel / 100) * (decimal) .25;
                        break;
                    }

                    Total_AddBacks += ab.amount;
                    Weighted_AddBacks += Effective_AddBack;

                    output.Effective_AddBacks = Effective_AddBack;
                    output.Total_AddBacks = Total_AddBacks;
                    output.Weighted_AddBacks = Weighted_AddBacks;

                }
            }
        }
    }

    public void calculateSDE()
    {
        decimal Base_SDE = 0, Risk_Adjusted_SDE = 0, Conservative_SDE = 0;
        foreach (object entry in _calculator.EntryList)
        {
            if(entry is Earning earning)
            {
                Base_SDE = earning.SDE + output.Total_AddBacks;
                Risk_Adjusted_SDE = earning.SDE + output.Weighted_AddBacks;
                Conservative_SDE = Risk_Adjusted_SDE * (decimal) 0.8;
            }
        }

        output.Base_SDE = Base_SDE;
        output.Risk_Adjusted_SDE = Risk_Adjusted_SDE;
        output.Conservative_SDE = Conservative_SDE;

    }

    public void displayCalculations()
    {
        
    }
    
}