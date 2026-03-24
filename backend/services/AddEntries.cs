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
        List<AddBack> addBacks = new List<AddBack>();

        bool cont = true;

        while(cont){
            
            AddBack addBack = new AddBack();

            Console.WriteLine("Owner claimed add backs exists (y/n)? ");

            if (Console.ReadLine().Equals("y"))
            {
                Console.WriteLine("Add Back Description: ");
                addBack.description = Console.ReadLine();

                Console.WriteLine("Add back amount: ");
                addBack.amount = decimal.Parse(Console.ReadLine());

                bool valid = false;

                while(!valid){

                    Console.WriteLine("Add back category: ");
                    string? category = Console.ReadLine();

                    switch (category)
                    {
                        case "discretionary":
                            addBack.category = AddBack.Category.discretionary;
                            valid = true;
                        break;

                        case "non-recurring":
                            addBack.category = AddBack.Category.non_recurring;
                            valid = true;
                        break;


                        case "questionable":
                            addBack.category = AddBack.Category.questionable;
                            valid = true;
                        break;

                        default:
                            Console.WriteLine("Invalid Category");
                        break;
                    }
                    
                }

                Console.WriteLine("Add back confidence level: ");
                addBack.confidenceLevel = int.Parse(Console.ReadLine());

                addBacks.Add(addBack);   

                }
            else
            {
                addBack.year = year;
                cont = false;
            }

        }

        _calculator.InputDictionary[year].Add(addBacks);

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
       
        List<Margin> profitFile = new List<Margin>();

        Margin profit = new Margin();
        
        Console.WriteLine("Please enter revenue amount: ");
        profit.revenue = decimal.Parse(Console.ReadLine());

        Console.WriteLine("Please enter expense amount: ");        
        profit.expense = decimal.Parse(Console.ReadLine());

        profitFile.Add(profit);         
        
        profit.year = year;
            
        _calculator.InputDictionary[year].Add(profitFile);
        
    }
    

    public void AccountingDetailEntry(int year)
    {
        AccountingDetail detail = new();

        detail.year = year;

        Console.WriteLine("Are EBITDA values avalibale? (y/n) ");
        string val = Console.ReadLine();

        switch (val)
        {
            case "y":
                detail.avaliableData = AccountingDetail.Avaliable.avaliable;

                Console.WriteLine("Please enter Interest Rate for year: ");
                detail.InterestRate = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Please enter Taxes for year: ");
                detail.Taxes = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Please enter Depreciation for year: ");
                detail.Depreciation = decimal.Parse(Console.ReadLine());

                Console.WriteLine("Please enter Amortization for year: ");
                detail.Amortization = decimal.Parse(Console.ReadLine());

            break;

            case "n":
                detail.avaliableData = AccountingDetail.Avaliable.not_avaliable;
            break;
        }
        
        _calculator.InputDictionary[year].Add(detail);

                
    }


    public void displayAllEntries()
    {
        foreach(KeyValuePair<int, List<object>> entry in _calculator.InputDictionary)
        {
            foreach(object list in entry.Value)
            {
                if(list is List<AddBack> a)
                {
                    AddBack ab = new();
                    ab.displayEntry(a);
                }
                else if (list is Earning ea)
                {
                    ea.displayEntry();

                }else if (list is List<Margin> mr)
                {   
                    Margin mg = new();
                    mg.displayEntry(mr);

                }

            }
        }
    }

}