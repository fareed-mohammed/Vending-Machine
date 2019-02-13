using System.Collections.Generic;

namespace VendingMachine
{
    public interface IProductInventoryRepository
    {
        Dictionary<string, int> GetInventory();
        void UpdateInventory(string code);
    }
}