using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VendingMachine.Interfaces;
using VendingMachine.UnitTests.Helpers;

namespace VendingMachine.UnitTests
{
    [TestClass]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepository;
        private Mock<IProductInventoryRepository> _productInventoryRepository;

        [TestInitialize]
        public void Initialise()
        {
            _productRepository = new Mock<IProductRepository>();
            _productInventoryRepository = new Mock<IProductInventoryRepository>();          
        }

        [TestMethod]
        public void ProductService_ProductRepository_IsNull_ExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new ProductService(null, _productInventoryRepository.Object), "Value cannot be null.\r\nParameter name: productRepository parameter is null");
        }

        [TestMethod]
        public void ProductService_ProductInventoryRepository_IsNull_ExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new ProductService(_productRepository.Object, null), "Value cannot be null.\r\nParameter name: productInventoryRepository parameter is null");
        }

        [TestMethod]
        public void ProductService_ProductRepository_GetAllProductsIsCalled_VerifyGetProductListIsCalledOnce()
        {
            // Arrange
            _productRepository.Setup(mock => mock.GetProductList()).Returns(new List<Product>());

            // Act
            var productService = new ProductService(_productRepository.Object, _productInventoryRepository.Object);

            var result = productService.GetAllProducts();

            // Assert
            _productRepository.Verify(mock => mock.GetProductList(), Times.Once);
        }

        [TestMethod]
        public void ProductService_GetProductIsCallled_VerifyGetProductListIsCalledOnce()
        {
            // Arrange
            _productRepository.Setup(mock => mock.GetProductList()).Returns(new List<Product>());

            // Act
            var productService = new ProductService(_productRepository.Object, _productInventoryRepository.Object);

            var result = productService.GetProduct(It.IsAny<string>());

            // Assert
            _productRepository.Verify(mock => mock.GetProductList(), Times.Once);
        }

        [TestMethod]
        public void ProductService_GetProduct_IsValidProductReturned()
        {
            // Arrange
            _productRepository.Setup(mock => mock.GetProductList()).Returns(CreateProducts());

            // Act
            var productService = new ProductService(_productRepository.Object, _productInventoryRepository.Object);

            var result = productService.GetProduct("COKE1");

            // Assert
            Assert.AreEqual(result != null, true);
            Assert.AreEqual(result.Code, "COKE1");
            Assert.AreEqual(result.Name, "Coke");
            Assert.AreEqual(result.Price, 1.00m);
            Assert.AreEqual(result.Type, ProductItemType.Coke);
        }

        [TestMethod]
        public void ProductService_GetProductQuantity_IsValidProductQuantityReturned()
        {
            // Arrange
            _productInventoryRepository.Setup(mock => mock.GetInventory()).Returns(CreateProductInventory());

            // Act
            var productService = new ProductService(_productRepository.Object, _productInventoryRepository.Object);

            var result = productService.GetProductQuantity("COKE1");

            // Assert
            Assert.AreEqual(result, 2);
        }

        [TestMethod]
        public void ProductService_UpdateProductQuantity_VerifyUpdateInventoryIsCalledOnce()
        {
            // Arrange
            _productInventoryRepository.Setup(mock => mock.UpdateInventory(It.IsAny<string>()));

            // Act
            var productService = new ProductService(_productRepository.Object, _productInventoryRepository.Object);

            productService.UpdateProductQuantity("COKE1");

            // Assert
            _productInventoryRepository.Verify(mock => mock.UpdateInventory(It.IsAny<string>()), Times.Once);
        }

        private List<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Code = "COKE1",
                    Name = "Coke",
                    Price = 1.00m,
                    Type = ProductItemType.Coke
                }               
            };
        }

        private Dictionary<string, int> CreateProductInventory()
        {
            return new Dictionary<string, int> {{"COKE1", 2}, {"PEPSI1", 0}, {"FANTA1", 10}, {"SPRITE1", 10}};
        }
    }
}
