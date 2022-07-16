using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XUnitTest.Web.Controllers;
using XUnitTest.Web.Models;
using XUnitTest.Web.Repositories;

namespace XUnitTest.Web.Test
{
    public class ProductApiControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepository;
        private readonly ProductsApiController _productsApiController;
        private readonly Helpers.Helper _helpers;
        private List<Product> products;
        public ProductApiControllerTest()
        {
            _mockRepository = new Mock<IRepository<Product>>();
            _productsApiController = new ProductsApiController(_mockRepository.Object);
            _helpers = new Helpers.Helper();
            products = new List<Product>() {
                new Product { Id = 1, Name = "Kalem", Stock = 50, Color = "Kırmızı" },
                new Product{ Id= 2 , Name = "Kitap", Stock = 100, Color = "Beyaz" }
            };
        }

        [Theory]
        [InlineData(5, 10, 15)]
        public void GetAdd_SampleValues_ReturnTotal(int a, int b, int total)
        {
            var result = _helpers.Add(a, b);
            Assert.Equal(total, result);
        }

        [Fact]
        public async void GetProduct_ActionExecutes_ReturnOkResultWithProduct()
        {
            _mockRepository.Setup(x => x.GetAll()).ReturnsAsync(products);
            var result = await _productsApiController.GetProduct();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal<int>(2, returnProducts.ToList().Count);
        }
        [Theory]
        [InlineData(0)]
        public async void GetProduct_InValidId_ReturnNotFoundResult(int productId)
        {
            Product product = null;
            _mockRepository.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _productsApiController.GetProduct(productId);
            Assert.IsType<NotFoundResult>(result);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetProduct_ValidId_ReturnOkResult(int productId)
        {
            var product = GetProduct(productId);
            _mockRepository.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _productsApiController.GetProduct(productId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(productId, returnProduct.Id);
            Assert.Equal(product.Name, returnProduct.Name);
        }
        [Theory]
        [InlineData(1)]
        public void PutProduct_IdIsNotEqualProduct_ReturnBadRequestResult(int productId)
        {
            var product = GetProduct(productId);
            var result = _productsApiController.PutProduct(2, product);
            Assert.IsType<BadRequestResult>(result);
        }
        [Theory]
        [InlineData(1)]
        public void PutProduct_ActionExecutes_ReturnNoContentResult(int productId)
        {
            var product = GetProduct(productId);
            _mockRepository.Setup(x => x.Update(product));
            var result = _productsApiController.PutProduct(productId, product);
            _mockRepository.Verify(x => x.Update(product), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
        [Theory]
        [InlineData(1)]
        public async void PostProduct_ActionExecutes_ReturnCreatedActionResult(int productId)
        {
            var product = GetProduct(productId);
            _mockRepository.Setup(x => x.Create(product)).Returns(Task.CompletedTask);
            var result = await _productsApiController.PostProduct(product);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

            _mockRepository.Verify(x => x.Create(product), Times.Once);

            Assert.Equal("GetProduct", createdAtActionResult.ActionName);
        }
        [Theory]
        [InlineData(0)]
        public async void DeleteProduct_InValidId_ReturnNotFoundResult(int productId)
        {
            Product product = null;
            _mockRepository.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var resultNotFound = await _productsApiController.DeleteProduct(productId);
            Assert.IsType<NotFoundResult>(resultNotFound.Result);
        }
        [Theory]
        [InlineData(1)]
        public async void DeleteProduct_ActionExecutes_ReturnNoContentResult(int productId)
        {
            var product = GetProduct(productId);
            _mockRepository.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            _mockRepository.Setup(x => x.Delete(product));
            var resultNoContent = await _productsApiController.DeleteProduct(productId);
            _mockRepository.Verify(x => x.Delete(product), Times.Once);
            Assert.IsType<NoContentResult>(resultNoContent.Result);
        }


        private Product GetProduct(int productId)
        {
            return products.First(x => x.Id == productId);
        }
    }
}
