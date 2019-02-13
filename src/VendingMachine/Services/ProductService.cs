using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductInventoryRepository _productInventoryRepository;
        public ProductService(IProductRepository productRepository, IProductInventoryRepository productInventoryRepository)
        {
            if (productRepository == null) throw new ArgumentNullException("productRepository parameter is null");
            if (productInventoryRepository == null) throw new ArgumentNullException("productInventoryRepository parameter is null");

            _productRepository = productRepository;
            _productInventoryRepository = productInventoryRepository;
        }

        public int GetProductQuantity(string code)
        {
            var quantities = _productInventoryRepository.GetInventory();
            return quantities[code];
        }

        public Product GetProduct(string code)
        {
            return GetAllProducts().FirstOrDefault(x => x.Code == code);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetProductList();
        }

        public void UpdateProductQuantity(string code)
        {            
            _productInventoryRepository.UpdateInventory(code);
        }        
    }
}