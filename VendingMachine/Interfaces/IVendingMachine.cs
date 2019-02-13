using System.Collections.Generic;

namespace VendingMachine
{
    public interface IVendingMachine
    {        
        VendingResponse AcceptCoin(InputCoin coin);
        VendingResponse SelectProduct(string code);
        IEnumerable<ItemChange> ReturnCoins();       
    }
}