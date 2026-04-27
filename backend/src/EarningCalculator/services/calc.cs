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
        calculateFinancialSummary();

        displayOutput();
    }

    public void calculateFinancialSummary()
    {
        try{ 

            decimal profit, SDE, margin, prevYear_revenue = 0;

            foreach(KeyValuePair<int, List<object>> entry in _calculator.InputDictionary.OrderBy(x => x.Key))
            {

                int year = entry.Key;
                CalculationOutput output = new() { year = year };

                foreach(var EntryClass in entry.Value)
                {
                    if(EntryClass is Margin mg)
                    {   
                        //calculating the profit
                        profit = mg.revenue - mg.expense;
                        output.Profit_Per_Year = profit;
                    

                        //calculating margin
                        margin = profit / mg.revenue;
                        output.Margin_Per_Year = margin;
                    

                        //checking if a previous year exist, if so calculate revenue growth percentage

                        decimal Revenue_Growth = prevYear_revenue == 0 
                                            ? 0 
                                            : (mg.revenue - prevYear_revenue) / prevYear_revenue;
                        
                        
                        prevYear_revenue = mg.revenue;

                        output.Revenue_Growth = Revenue_Growth;
                    
                    
                        //calculate EBITDTE per year
                        decimal EBITDA = calculateEBITDA(profit, year);
                        output.EBITDA = EBITDA;
                        
                        //calculate total, weighted, effective, conservative add back
                        calculateAddBacks(year, output);
                        
                        //using add backs to calculate base, risk, conservative SDE
                        calculateSDE(year, output); 
                                
                        //calculating adjusted profit
                        output.Yearly_Adjusted_Profit = profit + output.Weighted_AddBacks;

                        //calculating adjusted margin
                        output.Yearly_Adjusted_Margin = mg.revenue == 0
                                                ? 0
                                                : output.Yearly_Adjusted_Profit / mg.revenue;

                    }
                }

                _calculator.outputDictionary[year] = output;

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

        foreach (object entry in _calculator.InputDictionary[year]){
            
            if(entry is AccountingDetail list)
            {
                if(list.IsAvaliable){
                    Console.WriteLine("avaliable");
                    EBITDA_VALUE =  profit + list.InterestRate + list.Taxes + list.Depreciation + list.Amortization;
                }else{
                    EBITDA_VALUE = calculateEbitdaSDE(year);
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
                OwnerSalary = earning.OwnerSalary;

            }else if (entry is AddBack addBack)
            {
               non_operating_adjustments += addBack.AddBackTotalList?
               .Where(ab => ab.category == AddBack.AddBackArray.Category.non_recurring ||
               ab.category == AddBack.AddBackArray.Category.discretionary)
               .Sum(ab => ab.amount) ?? 0m;
            }
        }

        return SDE - OwnerSalary + non_operating_adjustments;
    }

    public void calculateAddBacks(int year, CalculationOutput output)
    {
        decimal Total_AddBacks = 0, Weighted_AddBacks = 0, conservative_AddBacks = 0;
        foreach (var entry in _calculator.InputDictionary[year]){
            
            if (entry is AddBack addBack)
            {
                if(addBack.AddBackTotalList != null)
                {
                    foreach(AddBack.AddBackArray ar in addBack.AddBackTotalList)
                    {
                        Weighted_AddBacks += ar.amount * (ar.confidenceLevel / 100m) * ar.categoryWeight;
                        Total_AddBacks += ar.amount;

                        if(ar.category == AddBack.AddBackArray.Category.non_recurring && ar.confidenceLevel >= 80)
                        {
                            conservative_AddBacks += ar.amount;
                        }

                    }
                }
            }
        }

        output.Conservative_AddBacks = conservative_AddBacks;
        output.Total_AddBacks = Total_AddBacks;
        output.Weighted_AddBacks = Weighted_AddBacks;

        output.Effective_AddBacks = Total_AddBacks == 0 
                                ? 0 
                                : Weighted_AddBacks / Total_AddBacks;

    }

    public void calculateSDE(int year, CalculationOutput output)
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

    public void displayOutput()
    {
         foreach(KeyValuePair<int, CalculationOutput> entry in _calculator.outputDictionary)
        {
            entry.Value.displayOutput();
        }
    }
    
}