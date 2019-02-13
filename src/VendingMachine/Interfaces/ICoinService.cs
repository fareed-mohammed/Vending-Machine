namespace VendingMachine
{
    public interface ICoinService
    {
        ValidCoin GetCoin(decimal weight, decimal diameter, decimal thickness);
    }
}