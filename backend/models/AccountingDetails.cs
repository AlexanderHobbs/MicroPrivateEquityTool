namespace EarningsCalculator;

public class AccountingDetail
{
    public int year {get; set;}
    public decimal InterestRate {get; set;}
    public decimal Taxes {get; set;}
    public decimal Depreciation {get; set;}
    public decimal Amortization {get; set;}

    public enum Avaliable
    {
        not_avaliable,
        avaliable
    }
    public Avaliable avaliableData {get; set;}
}