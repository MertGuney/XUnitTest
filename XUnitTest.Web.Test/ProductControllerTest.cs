using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using XUnitTest.Web.Controllers;
using XUnitTest.Web.Models;
using XUnitTest.Web.Repositories;

namespace XUnitTest.Web.Test
{
    public class ProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepository;
        private readonly ProductsController _productsController;
        private List<Product> products;
        public ProductControllerTest()
        {
            _mockRepository = new Mock<IRepository<Product>>();
            _productsController = new ProductsController(_mockRepository.Object);
            products = new List<Product>() {
                new Product { Id = 1, Name = "Kalem", Stock = 50, Color = "Kırmızı" },
                new Product{ Id= 2 , Name = "Kitap", Stock = 100, Color = "Beyaz" }
            };
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _productsController.Index();
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async void Index_ActionExecutes_ReturnProductList()
        {
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(products);
            var result = await _productsController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var productList = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
            Assert.Equal<int>(2, productList.Count());
        }
        [Fact]
        public async void Details_IdIsNull_ReturnRedirectToIndexAction()
        {
            var result = await _productsController.Details(null);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
        [Fact]
        public async void Details_IdInValid_ReturnNotFound()
        {
            Product product = null;
            _mockRepository.Setup(x => x.GetById(0)).ReturnsAsync(product);
            var result = await _productsController.Details(0);
            var redirect = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404, redirect.StatusCode);
        }
        [Theory]
        [InlineData(1)]
        public async void Details_ValidId_ReturnProduct(int productId)
        {
            Product product = products.First(x => x.Id == productId);
            _mockRepository.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _productsController.Details(productId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var resultProduct = Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Equal(product.Id, resultProduct.Id);
            Assert.Equal(product.Name, resultProduct.Name);
        }
        [Fact]
        public void Create_ActionExecutes_ReturnView()
        {
            var result = _productsController.Create();
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async void CreatePost_InValidModelState_ReturnView()
        {
            _productsController.ModelState.AddModelError("Name", "Name alanı gereklidir.");
            var result = await _productsController.Create(products.First());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Product>(viewResult.Model);
        }
        [Fact]
        public async void CreatePost_ValidModelState_ReturnRedirectToIndexAction()
        {
            var result = await _productsController.Create(products.First());
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
        [Fact]
        public async void CreatePost_ValidModelState_CreateMethodExecuted()
        {
            Product product = null;
            _mockRepository.Setup(x => x.Create(It.IsAny<Product>())).Callback<Product>(x => product = x);
            var result = await _productsController.Create(products.First());
            _mockRepository.Verify(x => x.Create(It.IsAny<Product>()), Times.Once);
            Assert.Equal(products.First().Id, product.Id);
        }
        [Fact]
        public async void CreatePost_InValidModelState_NeverCreateExecute()
        {
            _productsController.ModelState.AddModelError("Name", "");
            var result = await _productsController.Create(products.First());
            _mockRepository.Verify(x => x.Create(It.IsAny<Product>()), Times.Never);
        }
        [Fact]
        public void Edit_IsIdNull_ReturnRedirectToIndexAction()
        {
            var result = _productsController.Edit(null);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
        [Fact]
        public void Edit_IdInValid_ReturnNotFound()
        {
            Product product = null;
            _mockRepository.Setup(x => x.GetById(0)).ReturnsAsync(product);
            var result = _productsController.Edit(0);
            var redirect = Assert.IsType<NotFoundResult>(result);
            Assert.Equal<int>(404, redirect.StatusCode);
        }
        [Theory]
        [InlineData(1)]
        public void Edit_ValidId_ReturnProduct(int productId)
        {
            Product product = products.First(x => x.Id == productId);
            _mockRepository.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = _productsController.Edit(productId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var resultProduct = Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Equal(product.Id, resultProduct.Id);
        }
        [Theory]
        [InlineData(1)]
        public void EditPost_IdIsNotEqualProduct_ReturnNotFound(int productId)
        {
            var result = _productsController.Edit(2, products.First(x => x.Id == productId));
            var redirect = Assert.IsType<NotFoundResult>(result);
        }
        [Theory]
        [InlineData(1)]
        public void EditPost_InValidModelState_ReturnView(int productId)
        {
            _productsController.ModelState.AddModelError("Namee", "");
            var result = _productsController.Edit(productId, products.First(x => x.Id == productId));
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Product>(viewResult.Model);
        }
        [Theory]
        [InlineData(1)]
        public void EditPost_ValidModelState_ReturnRedirectToIndexAction(int productId)
        {
            var result = _productsController.Edit(productId, products.First(x => x.Id == productId));
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
        [Theory]
        [InlineData(1)]
        public void EditPost_ValidModelState_UpdateMethodExecuted(int productId)
        {
            var product = products.First(x => x.Id == productId);
            _mockRepository.Setup(x => x.Update(product));
            _productsController.Edit(productId, product);
            _mockRepository.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async void Delete_IdIsNull_ReturnNotFound()
        {
            var result = await _productsController.Delete(null);
            Assert.IsType<NotFoundResult>(result);
        }
        [Theory]
        [InlineData(0)]
        public async void Delete_IdInValid_ReturnNotFound(int productId)
        {
            Product product = null;
            _mockRepository.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _productsController.Delete(productId);
            Assert.IsType<NotFoundResult>(result);
        }
        [Theory]
        [InlineData(1)]
        public async void Delete_ActionExecutes_ReturnView(int productId)
        {
            var product = products.First(x => x.Id == productId);
            _mockRepository.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _productsController.Delete(productId);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Product>(viewResult.Model);
        }
        [Theory]
        [InlineData(1)]
        public async void DeleteConfirmed_ActionExecutes_ReturnRedirectToIndexAction(int productId)
        {
            var result = await _productsController.DeleteConfirmed(productId);
            Assert.IsType<RedirectToActionResult>(result);
        }
        [Theory]
        [InlineData(1)]
        public async void DeleteConfirmed_ActionExecutes_DeleteMethodExecute(int productId)
        {
            var product = products.First(x => x.Id == productId);
            _mockRepository.Setup(x => x.Delete(product));
            await _productsController.DeleteConfirmed(productId);
            _mockRepository.Verify(x => x.Delete(It.IsAny<Product>()), Times.Once);
        }
    }
}
