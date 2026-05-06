namespace DebtPaymentCalculator;

public class OutputDto
{
    public decimal SBA_AnnualPayment {get; set;}
    public decimal Seller_AnnualPayment {get; set;}
    public decimal AnnualDebtService {get; set;}
    public List<Amortization> AmortizationSchedule {get; set;} = new();
    public List<YearlyDebt> YearlyDebtPayments {get; set;} = new();
    public List<RemainingBalance> RemainingBalances {get; set;} = new();
    public decimal TotalInterestPaid {get; set;}
    public decimal TotalDebtBurden {get; set;}

    public class Amortization
    {  
        public int TermYear {get; set;}
        public decimal InterestPayment {get; set;}
        public decimal PrincipalPayment {get; set;}
        public decimal EndingBalance {get; set;}
        public decimal BeginningBalance {get; set;}

    };

    public class YearlyDebt
    {
        public int Year {get; set;}
        public decimal SBA_Payment {get; set;}
        public decimal Seller_Payment {get; set;}
        public decimal TotalPayment {get; set;}

    }

    public class RemainingBalance
    {
        public int Year {get; set;}
        public decimal SBA_Balance {get; set;}
        public decimal Seller_Balance {get; set;}
        public decimal TotalBalance {get; set;}
    }

// Output:
    // Exact yearly debt payments
    // Total interest paid
    // Remaining balance each year
    // Annual debt service 
    // amortization schedule
    // total debt burden
// Ex: “if I buy this for $1.5m, I will owe $x amount per yer”


}