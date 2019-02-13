using System.Collections.Generic;

namespace VendingMachine
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProductList();
    }
}