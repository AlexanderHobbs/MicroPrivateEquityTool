using Microsoft.VisualBasic;

namespace EarningsCalculator;

public class Calc : EarningCalculator
{
    CalculationOutput output;
    private readonly EarningCalculator _calculator;
    public Calc(EarningCalculator calculator)
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

            foreach(KeyValuePair<int, List<object>> entry in _calculator.InputDictionary)
            {
                CalculationOutput output = new();
                int year = entry.Key;
                foreach(var list in entry.Value)
                {
                    if(list is List<Margin> value)
                    {   
                        int index = 0;
                        foreach (Margin mg in value){
                        
                        //calculating the profit
                        profit = mg.revenue - mg.expense;
                        output.Profit_Per_Year = profit;
                    

                        //calculating margin
                        margin = profit / mg.revenue;
                        output.Margin_Per_Year = margin;
                    

                        //checking if a previous year exist, if so calculate revenue growth percentage
                        if(index-1 >= 0){
                            Margin prevYear = value[index-1];
                            Revenue_growth = (mg.revenue - prevYear.revenue) / prevYear.revenue;
                        }
                        else
                        {
                            Revenue_growth = -1;
                        }

                        index++;
                        output.Revenue_Growth = Revenue_growth;
                    

                        //calculate EBITDTE per year
                        EBIDTA = calculateEBITDA(profit, year);
                        output.EBITDA = EBIDTA;
                        
                        //calculate total, weighted, effective, conservative add back
                        calculateAddBacks(year);
                        
                        //using add backs to calculate base, risk, conservative SDE
                        calculateSDE(year); 
                                
                        //calculating adjusted profit
                        output.Yearly_Adjusted_Profit = profit + output.Weighted_AddBacks;

                        //calculating adjusted margin
                        output.Yearly_Adjusted_Margin = output.Yearly_Adjusted_Profit / mg.revenue;

                        }
                    }
                }

                if (!_calculator.outputDictionary.ContainsKey(year))
                {
                    _calculator.outputDictionary.Add(year, new List<object>());
                }

                _calculator.outputDictionary[year].Add(output);
            }
        }catch (Exception e)
        {
            Console.WriteLine($"Exception: {e}");
        }
    }

    public void calculateMarginTrend()
    {
    
    }

    public void calculateRevenueTrend()
    {
        
    }

    public decimal calculateEBITDA(decimal profit, int year)
    {
        decimal EBITDA_VALUE = 0;
        decimal SDE = 0, OwnerSalary = 0, non_operating_adjustments = 0;

        foreach (object entry in _calculator.InputDictionary[year]){
            
            if(entry is AccountingDetail list)
            {
                switch (list.avaliableData)
                {
                    case AccountingDetail.Avaliable.avaliable:
                        Console.WriteLine("avaliable");
                        EBITDA_VALUE =  profit + list.InterestRate + list.Taxes + list.Depreciation + list.Amortization;
                        break;

                    case AccountingDetail.Avaliable.not_avaliable:
                        EBITDA_VALUE = calculateEbitdaSDE(year);
                        break;
                }
            }
        }

        return EBITDA_VALUE;

    }

    public decimal calculateEbitdaSDE(int year)
    {
        decimal SDE = 0, OwnerSalary = 0, non_operating_adjustments = 0;

        foreach (object entry in _calculator.InputDictionary[year]){
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

    public void calculateAddBacks(int year)
    {
        decimal Effective_AddBack = 0, Total_AddBacks = 0, Weighted_AddBacks = 0, conservative_AddBacks = 0;
        foreach (object entry in _calculator.InputDictionary[year]){
            
            if (entry is List<AddBack> addBack)
            {
                foreach(AddBack ab in addBack)
                {
                    switch (ab.category)
                    {
                        case AddBack.Category.non_recurring:
                            Effective_AddBack = ab.amount * (ab.confidenceLevel / 100) * (decimal) 1.0;
                            if(ab.confidenceLevel > 90)
                            {
                                conservative_AddBacks += ab.amount;
                            }
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
                    output.Conservative_AddBacks = conservative_AddBacks;

                }
            }
        }
    }

    public void calculateSDE(int year)
    {
        decimal Base_SDE = 0, Risk_Adjusted_SDE = 0, Conservative_SDE = 0;
        foreach (object entry in _calculator.InputDictionary[year])
        {
            if(entry is Earning earning)
            {
                Base_SDE = earning.SDE + output.Total_AddBacks;
                Risk_Adjusted_SDE = earning.SDE + output.Weighted_AddBacks;
                Conservative_SDE = earning.SDE * output.Conservative_AddBacks;
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