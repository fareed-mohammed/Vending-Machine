using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VendingMachine.Interfaces;
using VendingMachine.UnitTests.Helpers;

namespace VendingMachine.UnitTests
{
    [TestClass]
    public class VendingMachineTests
    {
        private Mock<IProductService> _productService;
        private Mock<ICoinService> _coinService;

        [TestInitialize]
        public void Initialise()
        {
            _productService = new Mock<IProductService>();
            _coinService = new Mock<ICoinService>();          
        }

        [TestMethod]
        public void VendingMachine_CoinService_IsNull_ExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new VendingMachine(null, _productService.Object), "Value cannot be null.\r\nParameter name: coinService parameter is null");
        }

        [TestMethod]
        public void VendingMachine_ProductService_IsNull_ExceptionThrown()
        {
            // Arrange + Act + Assert
            AssertException.Throws<ArgumentNullException>(() => new VendingMachine(_coinService.Object, null), "Value cannot be null.\r\nParameter name: productService parameter is null");
        }

        [TestMethod]
        public void VendingMachine_AcceptCoin_NullCoinEntered()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);

            // Assert
            AssertException.Throws<ArgumentNullException>(() => vendingMachine.AcceptCoin(null), "Value cannot be null.\r\nParameter name: Coin parameter null!");
        }

        [TestMethod]
        public void VendingMachine_AcceptCoin_InvalidCoinEntered()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.AcceptCoin(CreateInvalidCoin());

            // Assert
            Assert.AreEqual(result != null, true);
            Assert.AreEqual(result.IsRejectedCoin, true);
        }

        [TestMethod]
        public void VendingMachine_AcceptCoin_ValidCoinEntered()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedFivePenceCoin);

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.AcceptCoin(CreateValidFivePenceCoin());

            // Assert
            Assert.AreEqual(result != null, true);
            Assert.AreEqual(result.IsRejectedCoin, false);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.Message, "0.05");

        }

        [TestMethod]
        public void VendingMachine_AcceptCoin_VerifyCoinServiceIsCalledOnce()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()));

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.AcceptCoin(CreateInvalidCoin());

            // Assert
            _coinService.Verify(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_InvalidCode_ExceptionThrown()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);

            // Assert
            AssertException.Throws<ArgumentNullException>(() => vendingMachine.SelectProduct(""), "Value cannot be null.\r\nParameter name: Code parameter empty!");
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_InvalidCodeEntered()
        {
            // Arrange
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("INVALIDCODE");

            // Assert
            Assert.AreEqual(result != null, true);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, "Invalid Product Selected. Please try again");
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_InvalidCodeEntered_VerifyGetProductCalledOnce()
        {
            // Arrange
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(() => null);

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("INVALIDCODE");

            // Assert
            _productService.Verify(mock => mock.GetProduct(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_NoCoinsEntered()
        {
            // Arrange
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("COKE1");

            // Assert
            Assert.AreEqual(result != null, true);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, "Insert Coin");
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_NoCoinsEntered_VerifyGetProductCalledOnce()
        {
            // Arrange
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);

            VendingResponse result = vendingMachine.SelectProduct("COKE1");

            // Assert
            _productService.Verify(mock => mock.GetProduct(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_CoinsEntered_LessThanProductPrice()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedFivePenceCoin);
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidFivePenceCoin());

            VendingResponse result = vendingMachine.SelectProduct("COKE1");

            // Assert
            Assert.AreEqual(result != null, true);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, string.Format("Price : {0}", 1.00m));
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_CoinsEntered_IsValid()
        {
            // Arrange
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedOnePoundCoin);
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            _productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            _productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());

            VendingResponse result = vendingMachine.SelectProduct("COKE1");

            // Assert
            Assert.AreEqual(result != null, true);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.Message, "Thank You");
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_CoinsEnteredAndReturnCoins_IsValid()
        {           
            // Arrange            
            _coinService.Setup(mock => mock.GetCoin(9.5m, 22.5m, 3.15m)).Returns(CreateValidAcceptedOnePoundCoin);
            _coinService.Setup(mock => mock.GetCoin(3.25m, 18.0m, 1.7m)).Returns(CreateValidAcceptedFivePenceCoin);
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            _productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            _productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());
            vendingMachine.AcceptCoin(CreateValidFivePenceCoin());

            VendingResponse result = vendingMachine.SelectProduct("COKE1");

            // Assert
            Assert.AreEqual(result != null, true);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.Message, "Thank You");
            Assert.AreEqual(result.Change != null, true);
            Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.FivePence).Number, 1);
            Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.TwentyPence) == null, true);
            Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.FiftyPence) == null, true);
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_CoinsEnteredAndReturnMoreThanOneCoin_IsValid()
        {
            // Arrange            
            _coinService.Setup(mock => mock.GetCoin(9.5m, 22.5m, 3.15m)).Returns(CreateValidAcceptedOnePoundCoin);
            _coinService.Setup(mock => mock.GetCoin(3.25m, 18.0m, 1.7m)).Returns(CreateValidAcceptedFivePenceCoin);
            _coinService.Setup(mock => mock.GetCoin(5.0m, 21.4m, 1.7m)).Returns(CreateValidAcceptedTwentyPenceCoin);
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            _productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            _productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());
            vendingMachine.AcceptCoin(CreateValidFivePenceCoin());
            vendingMachine.AcceptCoin(CreateValidTwentyPenceCoin());

            VendingResponse result = vendingMachine.SelectProduct("COKE1");

            // Assert
            Assert.AreEqual(result != null, true);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.Message, "Thank You");
            Assert.AreEqual(result.Change != null, true);
            Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.FivePence).Number, 1);
            Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.TwentyPence).Number, 1);
            Assert.AreEqual(result.Change.SingleOrDefault(item => item.Type == CoinType.FiftyPence) == null, true);
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_CoinsEnteredAndReturned_VerifyGetProductQuantityCalledOnce()
        {
            // Arrange            
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedOnePoundCoin);           
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            _productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            _productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());           

            VendingResponse result = vendingMachine.SelectProduct("COKE1");

            // Assert
            _productService.Verify(mock => mock.GetProductQuantity(It.IsAny<string>()), Times.Once);          
        }

        [TestMethod]
        public void VendingMachine_SelectProduct_CoinsEnteredAndReturned_VerifyUpdateProductQuantityCalledOnce()
        {
            // Arrange            
            _coinService.Setup(mock => mock.GetCoin(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(CreateValidAcceptedOnePoundCoin);           
            _productService.Setup(mock => mock.GetProduct(It.IsAny<string>())).Returns(CreateProduct());
            _productService.Setup(mock => mock.GetProductQuantity(It.IsAny<string>())).Returns(10);
            _productService.Setup(mock => mock.UpdateProductQuantity(It.IsAny<string>()));

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());          

            VendingResponse result = vendingMachine.SelectProduct("COKE1");

            // Assert           
            _productService.Verify(mock => mock.UpdateProductQuantity(It.IsAny<string>()), Times.Once);            
        }

        [TestMethod]
        public void VendingMachine_ReturnCoins_CoinsEnteredAndReturned_IsValid()
        {
            // Arrange            
            _coinService.Setup(mock => mock.GetCoin(9.5m, 22.5m, 3.15m)).Returns(CreateValidAcceptedOnePoundCoin);
            _coinService.Setup(mock => mock.GetCoin(3.25m, 18.0m, 1.7m)).Returns(CreateValidAcceptedFivePenceCoin);
            _coinService.Setup(mock => mock.GetCoin(5.0m, 21.4m, 1.7m)).Returns(CreateValidAcceptedTwentyPenceCoin);           

            // Act
            var vendingMachine = new VendingMachine(_coinService.Object, _productService.Object);
            vendingMachine.AcceptCoin(CreateValidOnePoundCoin());
            vendingMachine.AcceptCoin(CreateValidFivePenceCoin());
            vendingMachine.AcceptCoin(CreateValidTwentyPenceCoin());

            var result = vendingMachine.ReturnCoins();

            // Assert
            Assert.AreEqual(result != null, true);
            Assert.AreEqual(result.SingleOrDefault(item => item.Type == CoinType.FivePence).Number, 1);
            Assert.AreEqual(result.SingleOrDefault(item => item.Type == CoinType.TwentyPence).Number, 1);
            Assert.AreEqual(result.SingleOrDefault(item => item.Type == CoinType.OnePound).Number, 1);
        }

        private InputCoin CreateValidFivePenceCoin()
        {
            return new InputCoin()
            {
                Diameter = 18.0m,
                Thickness = 1.7m,
                Weight = 3.25m
            };
        }

        private InputCoin CreateValidTwentyPenceCoin()
        {
            return new InputCoin()
            {
                Diameter = 21.4m,
                Thickness = 1.7m,                
                Weight = 5.0m
            };
        }
        private InputCoin CreateValidOnePoundCoin()
        {
            return new InputCoin()
            {
                Diameter = 22.5m,
                Thickness = 3.15m,                
                Weight = 9.5m
            };
        }
        private InputCoin CreateInvalidCoin()
        {
            return new InputCoin()
            {
                Diameter = 999m,
                Thickness = 999m,
                Weight = 999m
            };
        }

        private ValidCoin CreateValidAcceptedFivePenceCoin()
        {
            return new ValidCoin()
            {
                Diameter = 18.0m,
                Thickness = 1.7m,
                Weight = 3.25m,
                Type = CoinType.FivePence,
                Value = 0.05m
            };
        }

        private ValidCoin CreateValidAcceptedTwentyPenceCoin()
        {
            return new ValidCoin()
            {
                Diameter = 21.4m,
                Thickness = 1.7m,
                Type = CoinType.TwentyPence,
                Weight = 5.0m,
                Value = 0.20m
            };
        }

        private ValidCoin CreateValidAcceptedOnePoundCoin()
        {
            return new ValidCoin()
            {
                Diameter = 22.5m,
                Thickness = 3.15m,
                Type = CoinType.OnePound,
                Weight = 9.5m,
                Value = 1.00m
            };
        }

        private Product CreateProduct()
        {
            return new Product
            {
                Name = "COKE",
                Code = "COKE1",
                Price = 1.00m,
                Type = ProductItemType.Coke
            };
        }
    }
}
