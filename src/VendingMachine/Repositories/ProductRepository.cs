using System.Collections;
using System.Collections.Generic;

namespace VendingMachine
{
    public class ProductRepository : IProductRepository
    {
        private static List<Product> _products;
        
        public IEnumerable<Product> GetProductList()
        {
            return _products ?? (_products = new List<Product>
            {
                new Product() {Code = "COKE1", Type = ProductItemType.Coke, Name = "Coke", Price = 1.00m},
                new Product() {Code = "PEPSI1", Type = ProductItemType.Pepsi, Name = "Pepsi", Price = 1.00m},
                new Product() {Code = "FANTA1", Type = ProductItemType.Fanta, Name = "Fanta", Price = 0.75m},
                new Product() {Code = "SPRITE1", Type = ProductItemType.Sprite, Name = "Sprite", Price = 1.00m}
            });
        }
    }
}