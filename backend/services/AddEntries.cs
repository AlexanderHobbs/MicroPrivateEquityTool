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
                    string? cat = Console.ReadLine();

                    if (cat.Equals("discretionary"))
                    {
                        addBack.category = AddBack.Category.discretionary;
                        valid = true;
                    }else if (cat.Equals("non-recurring"))
                    {
                        addBack.category = AddBack.Category.non_recurring;
                        valid = true;
                    }
                    else if(cat.Equals("questionable"))
                    {
                        addBack.category = AddBack.Category.questionable;
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Category");
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
        _calculator.EntryList.Add(addBacks);

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
        _calculator.EntryList.Add(compensation);

    }
    

    public void marginEntry(int year)
    {
       
        List<Margin> profitFile = new List<Margin>();

        bool cont = true;

        while(cont){

            Margin profit = new Margin();
            
            Console.WriteLine("Please enter revenue amount: ");
            profit.revenue = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Please enter expense amount: ");        
            profit.expense = decimal.Parse(Console.ReadLine());

            profitFile.Add(profit);


            Console.WriteLine("Add another entry (y/n)? ");
            
            if (Console.ReadLine().Equals("n"))
            {
                profit.year = year;
                cont = false;
            }
        }

        
        
        _calculator.EntryList.Add(profitFile);
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
        
        _calculator.EntryList.Add(detail);
        _calculator.InputDictionary[year].Add(detail);

                
    }

    // public void marginEntry2()
    // {
       
    //     Margin profit;

    //     bool cont = true;

    //     while(cont){

    //         profit = new Margin();

    //         Console.WriteLine("Please enter year: ");
    //         int year = int.Parse(Console.ReadLine());

    //         if (!Margin.Dictionary.ContainsKey(year))
    //         {
    //             Margin.Dictionary.Add(year, new Margin());
    //         }            

    //         Console.WriteLine("Please enter revenue amount: ");
    //         profit.revenue = float.Parse(Console.ReadLine());

    //         Console.WriteLine("Please enter expense amount: ");        
    //         profit.expense = float.Parse(Console.ReadLine());

    //         Margin.Dictionary[year] = profit;

    //         Console.WriteLine("Add another entry (y/n)? ");
            
    //         if (Console.ReadLine().Equals("n"))
    //         {
    //             cont = false;
    //         }
    //     }
        
    //     _calculator.EntryList.Add(Margin.Dictionary);

    // }


    public void displayEntry()
    {
            foreach(var item in _calculator.EntryList)
            {
                if(item is List<AddBack> a)
                {   
                    AddBack ab = new();
                    ab.displayEntry(a);

                }else if (item is Earning ea)
                {
                    ea.displayEntry();

                }else if (item is List<Margin> mr)
                {   
                    Margin mg = new();
                    mg.displayEntry(mr);

                }

                Console.WriteLine("\n--------------------------------------------------\n");

            }
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