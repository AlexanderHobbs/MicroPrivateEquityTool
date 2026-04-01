using System.Formats.Asn1;

namespace EarningsCalculator;

public class AddEntry : EarningCalculator
{
    private readonly EarningCalculator _calculator;
    public AddEntry(EarningCalculator calculator)
    {
        _calculator =  calculator;
    }
    public void add()
    {
        bool cont = true;

        while(cont){
            Console.WriteLine("Please Enter Year: ");
            int year = int.Parse(Console.ReadLine());

            if (!_calculator.InputDictionary.ContainsKey(year))
            {
                _calculator.InputDictionary.Add(year, new List<object>());
            }

            marginEntry(year);
            earningsEntry(year);
            AddBackEntry(year);
            AccountingDetailEntry(year);

            Console.WriteLine("Would you like to add another year of data? (y/n)");
            if (Console.ReadLine().Equals("n"))
            {
                cont = false;
            }
        }

        displayAllEntries();

        
    }


    public void AddBackEntry(int year)
    {

        AddBack addBack = new AddBack();

        bool cont = true;

        while(cont){
            
            Console.WriteLine("Owner claimed add backs exists (y/n)? ");

            if (Console.ReadLine().Equals("y"))
            {
                
                addBack.addBacks_present = true;
                AddBack.AddBackArray newAddBackList = new AddBack.AddBackArray();

                Console.WriteLine("Add Back Description: ");
                newAddBackList.description = Console.ReadLine();

                Console.WriteLine("Add back amount: ");
                newAddBackList.amount = decimal.Parse(Console.ReadLine());

                bool valid = false;

                while(!valid){

                    Console.WriteLine("Add back category: ");
                    string? category = Console.ReadLine().ToUpper();

                    switch (category)
                    {
                        case "DISCRETIONARY":
                            newAddBackList.category = AddBack.AddBackArray.Category.discretionary;
                            newAddBackList.categoryWeight = 0.75M;
                            valid = true;
                        break;

                        case "NON-RECURRING":
                            newAddBackList.category = AddBack.AddBackArray.Category.non_recurring;
                            newAddBackList.categoryWeight = 1.0M;
                            valid = true;
                        break;


                        case "QUESTIONABLE":
                            newAddBackList.category = AddBack.AddBackArray.Category.questionable;
                            newAddBackList.categoryWeight = 0.25M;
                            valid = true;
                        break;

                        default:
                            Console.WriteLine("Invalid Category");
                        break;
                    }
                    
                }

                Console.WriteLine("Add back confidence level: ");
                newAddBackList.confidenceLevel = int.Parse(Console.ReadLine());
                
                addBack.AddBackTotalList.Add(newAddBackList);

                }

            else
            {
                addBack.year = year;
                cont = false;
            }

        }

        _calculator.InputDictionary[year].Add(addBack);

    }


    public void earningsEntry(int year)
    {   

        Earning compensation = new Earning();

        compensation.year = year;

        Console.WriteLine("Reported SDE: ");
        compensation.SDE = decimal.Parse(Console.ReadLine());

        Console.WriteLine("Reported Salary: ");
        compensation.OwnerSalary = decimal.Parse(Console.ReadLine());

        _calculator.InputDictionary[year].Add(compensation);

    }
    

    public void marginEntry(int year)
    {
       
        Margin profit = new Margin();
        
        Console.WriteLine("Please enter revenue amount: ");
        profit.revenue = decimal.Parse(Console.ReadLine());

        Console.WriteLine("Please enter expense amount: ");        
        profit.expense = decimal.Parse(Console.ReadLine());

        profit.year = year;
            
        _calculator.InputDictionary[year].Add(profit);
        
    }
    

    public void AccountingDetailEntry(int year)
    {
        AccountingDetail detail = new();

        detail.year = year;

        Console.WriteLine("Are EBITDA values avalibale? (y/n) ");
        string val = Console.ReadLine();

        if(val.Equals("y")){
            detail.IsAvaliable = true;

            Console.WriteLine("Please enter Interest Rate for year: ");
            detail.InterestRate = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Please enter Taxes for year: ");
            detail.Taxes = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Please enter Depreciation for year: ");
            detail.Depreciation = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Please enter Amortization for year: ");
            detail.Amortization = decimal.Parse(Console.ReadLine());

        }else{
            detail.IsAvaliable = false;
        }
        
        _calculator.InputDictionary[year].Add(detail);

                
    }


    public void displayAllEntries()
    {
        foreach(KeyValuePair<int, List<object>> entry in _calculator.InputDictionary)
        {
            
            foreach(object list in entry.Value)
            {
                if(list is AddBack ab)
                {
                    ab.displayEntry();
                }
                else if (list is Earning ea)
                {
                    ea.displayEntry();

                }else if (list is Margin mr)
                {   
                    mr.displayEntry();

                }else if (list is AccountingDetail dt)
                {
                    dt.displayEntry();
                }

            }
        }
    }

}