using FakeItEasy;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopingList.Controllers;
using ShopingList.Data.Models;
using ShopingList.Models;
using ShopingList.Services;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;

namespace ShopingListTest
{
    public class ProductControllerTests
    {
        private static IProductService productService;
        private static ICategoryService categoryService;
        private static ProductsController productsController;

        [SetUp]
        public void Setup()
        {
            productService = A.Fake<IProductService>();
            categoryService = A.Fake<ICategoryService>();
            productsController = new ProductsController(productService, categoryService);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("test-custom-claim", "test claim value"),
           }, "mock"));

            productsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task Index_ShouldReturn_ViewModel_ProductsList()
        {
            //Arrange
            A.CallTo(() => productService.GetAllProducts()).Returns(new List<Product>
            {
                new Product
                {
                    Category =A.Fake<ProductCategory>()
                },
                new Product
                {
                     Category =A.Fake<ProductCategory>()
                },
                new Product
                {
                     Category =A.Fake<ProductCategory>()
                }

            });

            //Act
            var response = productsController.Index();
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsInstanceOf(typeof(ViewResult), actualResult);
            Assert.IsNotNull(actualResult);
            var model = actualResult.Model as List<ProductVM>;
            Assert.IsNotNull(model);
            Assert.That(model.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task Create_ShoudReturn_ViewModel_CategoriesList()
        {
            //Arrange
            A.CallTo(() => categoryService.GetAllProductCategories()).Returns(A.CollectionOfFake<ProductCategory>(5));

            //Act
            var response = productsController.Create();
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            var selectedList = actualResult.ViewData.Values.ToList()[0] as SelectList;
            Assert.IsNotNull(selectedList);
            Assert.That(selectedList.Count, Is.EqualTo(6));
        }

        [Test]
        public async Task Create_ShoudReturn_Index()
        {
            //Arrange
            var product = A.Fake<ProductModel>();

            //Act
            var response = productsController.Create(product);
            var actualResult = await response as RedirectToActionResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Create_RequerdFieldName_ShoudReturn_ModelStateError_And_CategoriesList()
        {
            //Arrange
            A.CallTo(() => categoryService.GetAllProductCategories()).Returns(A.CollectionOfFake<ProductCategory>(5));

            var product = new ProductModel() 
            {
                Id = 1
            };

            string modelStateErrorKey = "Name";
            productsController.ModelState.AddModelError(modelStateErrorKey, "Name is required.");

            //Act
            var response = productsController.Create(product);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            var selectedList = actualResult.ViewData.Values.ToList()[0] as SelectList;
            Assert.IsNotNull(selectedList);
            Assert.That(selectedList.Count, Is.EqualTo(6));
            Assert.IsNotNull(actualResult.ViewData);
            Assert.IsNotNull(actualResult.ViewData.ModelState[modelStateErrorKey]);
            var erorrsCount = actualResult.ViewData.ModelState[modelStateErrorKey].Errors.Count;
            Assert.That(erorrsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Create_MaxLength100_Name_ShoudReturn_ModelStateError_And_CategoriesList()
        {
            //Arrange
            A.CallTo(() => categoryService.GetAllProductCategories()).Returns(A.CollectionOfFake<ProductCategory>(5));

            var product = new ProductModel()
            {
                Id = 1,
                Name = "C7zF5zKS53bL2FPamwqeZ02kXUNm3NGGV6bhqDfdMqrbWuu8grcxWSuO1oKGSPbztZ3koNmbUhV9sviEm1FHj35Skqq55Mq4wrqa1234"
            };

            string modelStateErrorKey = "Name";
            productsController.ModelState.AddModelError(modelStateErrorKey, "The product name should be less than 100 symbols!");

            //Act
            var response = productsController.Create(product);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            var selectedList = actualResult.ViewData.Values.ToList()[0] as SelectList;
            Assert.IsNotNull(selectedList);
            Assert.That(selectedList.Count, Is.EqualTo(6));
            Assert.IsNotNull(actualResult.ViewData);
            Assert.IsNotNull(actualResult.ViewData.ModelState[modelStateErrorKey]);
            var erorrCount = actualResult.ViewData.ModelState[modelStateErrorKey].Errors.Count;
            var erorrMsg = actualResult.ViewData.ModelState[modelStateErrorKey].Errors[0].ErrorMessage;
            Assert.That(erorrMsg, Is.EqualTo("The product name should be less than 100 symbols!"));
            Assert.That(erorrCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Create_RequiredCategory_ShoudReturn_ModelStateError_And_CategoriesList()
        {
            //Arrange
            A.CallTo(() => categoryService.GetAllProductCategories()).Returns(A.CollectionOfFake<ProductCategory>(5));

            var product = new ProductModel()
            {
                Id = -1,
                Name = "Product"
            };

            string modelStateErrorKey = "CategoryId";
            productsController.ModelState.AddModelError(modelStateErrorKey, "Please choose product category!");

            //Act
            var response = productsController.Create(product);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            var selectedList = actualResult.ViewData.Values.ToList()[0] as SelectList;
            Assert.IsNotNull(selectedList);
            Assert.That(selectedList.Count, Is.EqualTo(6));
            Assert.IsNotNull(actualResult.ViewData);
            Assert.IsNotNull(actualResult.ViewData.ModelState[modelStateErrorKey]);
            var erorrCount = actualResult.ViewData.ModelState[modelStateErrorKey].Errors.Count;
            var erorrMsg = actualResult.ViewData.ModelState[modelStateErrorKey].Errors[0].ErrorMessage;
            Assert.That(erorrMsg, Is.EqualTo("Please choose product category!"));
            Assert.That(erorrCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Edit_ShouldReturn_NotFount() 
        {
            //Arrange
            A.CallTo(() => productService.GetProductById(A<int>.Ignored)).Returns(new Product().Id == 0 ? null : new Product());

            //Act
            var response = productsController.Edit(0);
            var actualResult = await response as NotFoundResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task Edit_ShouldReturn_Product_And_CategoriesList()
        {
            //Arrange
            A.CallTo(() => productService.GetProductById(A<int>.Ignored)).Returns(A.Fake<Product>());
            A.CallTo(() => categoryService.GetAllProductCategories()).Returns(A.CollectionOfFake<ProductCategory>(5));

            //Act
            var response = productsController.Edit(0);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.ViewData.Model);
            var selectedList = actualResult.ViewData.Values.ToList()[0] as SelectList;
            Assert.IsNotNull(selectedList);
            Assert.That(selectedList.Count, Is.EqualTo(5));
        }

        [Test]
        public async Task EditProduct_ShouldReturn_NotFount()
        {
            //Arrange
            var fakeProduct = A.Fake<ProductModel>();

            //Act
            var response = productsController.Edit(-1, fakeProduct);
            var actualResult = await response as NotFoundResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task Edit_RequerdFieldName_ShoudReturn_ModelStateError_And_CategoriesList()
        {
            //Arrange
            A.CallTo(() => categoryService.GetAllProductCategories()).Returns(A.CollectionOfFake<ProductCategory>(5));

            var product = new ProductModel()
            {
                Id = 1
            };

            string modelStateErrorKey = "Name";
            productsController.ModelState.AddModelError(modelStateErrorKey, "Name is required.");

            //Act
            var response = productsController.Edit(1, product);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            var selectedList = actualResult.ViewData.Values.ToList()[0] as SelectList;
            Assert.IsNotNull(selectedList);
            Assert.That(selectedList.Count, Is.EqualTo(5));
            Assert.IsNotNull(actualResult.ViewData);
            Assert.IsNotNull(actualResult.ViewData.ModelState[modelStateErrorKey]);
            var erorrsCount = actualResult.ViewData.ModelState[modelStateErrorKey].Errors.Count;
            Assert.That(erorrsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Edit_RequiredCategory_ShoudReturn_ModelStateError_And_CategoriesList()
        {
            //Arrange
            A.CallTo(() => categoryService.GetAllProductCategories()).Returns(A.CollectionOfFake<ProductCategory>(5));

            var product = new ProductModel()
            {
                Id = -1,
                Name = "Product"
            };

            string modelStateErrorKey = "CategoryId";
            productsController.ModelState.AddModelError(modelStateErrorKey, "Please choose product category!");

            //Act
            var response = productsController.Edit(-1, product);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            var selectedList = actualResult.ViewData.Values.ToList()[0] as SelectList;
            Assert.IsNotNull(selectedList);
            Assert.That(selectedList.Count, Is.EqualTo(5));
            Assert.IsNotNull(actualResult.ViewData);
            Assert.IsNotNull(actualResult.ViewData.ModelState[modelStateErrorKey]);
            var erorrCount = actualResult.ViewData.ModelState[modelStateErrorKey].Errors.Count;
            var erorrMsg = actualResult.ViewData.ModelState[modelStateErrorKey].Errors[0].ErrorMessage;
            Assert.That(erorrMsg, Is.EqualTo("Please choose product category!"));
            Assert.That(erorrCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Edit_MaxLength100_Name_ShoudReturn_ModelStateError_And_CategoriesList()
        {
            //Arrange
            A.CallTo(() => categoryService.GetAllProductCategories()).Returns(A.CollectionOfFake<ProductCategory>(5));

            var product = new ProductModel()
            {
                Id = 1,
                Name = "C7zF5zKS53bL2FPamwqeZ02kXUNm3NGGV6bhqDfdMqrbWuu8grcxWSuO1oKGSPbztZ3koNmbUhV9sviEm1FHj35Skqq55Mq4wrqa1234"
            };

            string modelStateErrorKey = "Name";
            productsController.ModelState.AddModelError(modelStateErrorKey, "The product name should be less than 100 symbols!");

            //Act
            var response = productsController.Edit(1, product);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            var selectedList = actualResult.ViewData.Values.ToList()[0] as SelectList;
            Assert.IsNotNull(selectedList);
            Assert.That(selectedList.Count, Is.EqualTo(5));
            Assert.IsNotNull(actualResult.ViewData);
            Assert.IsNotNull(actualResult.ViewData.ModelState[modelStateErrorKey]);
            var erorrCount = actualResult.ViewData.ModelState[modelStateErrorKey].Errors.Count;
            var erorrMsg = actualResult.ViewData.ModelState[modelStateErrorKey].Errors[0].ErrorMessage;
            Assert.That(erorrMsg, Is.EqualTo("The product name should be less than 100 symbols!"));
            Assert.That(erorrCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Delete_ShouldReturn_NotFount()
        {
            //Arrange
            A.CallTo(() => productService.GetProductById(A<int>.Ignored)).Returns(new Product().Id == 0 ? null : new Product());

            //Act
            var response = productsController.Delete(0);
            var actualResult = await response as NotFoundResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task Delete_ShouldReturn_Product_And_CategoriesList()
        {
            //Arrange
            A.CallTo(() => productService.GetProductById(A<int>.Ignored)).Returns(A.Fake<Product>());
            A.CallTo(() => categoryService.GetAllProductCategories()).Returns(A.CollectionOfFake<ProductCategory>(5));

            //Act
            var response = productsController.Delete(0);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.ViewData.Model);
            var selectedList = actualResult.ViewData.Values.ToList()[0] as SelectList;
            Assert.IsNotNull(selectedList);
            Assert.That(selectedList.Count, Is.EqualTo(5));
        }

        [Test]
        public async Task DeleteConfirm_ShoudReturn_Index()
        {
            //Arrange
            A.CallTo(() => productService.GetProductById(A<int>.Ignored)).Returns(A.Fake<Product>());

            //Act
            var response = productsController.DeleteConfirmed(1);
            var actualResult = await response as RedirectToActionResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task GetProductListByPrefix() 
        {
            //Arrange
            A.CallTo(() => productService.GetProductsByPrefix(A<string>.Ignored)).Returns(A.CollectionOfFake<Product>(3));

            //Act
            var response = productsController.GetProductsList(String.Empty);
            var actualResult = await response as JsonResult;

            //Assert
            Assert.IsNotNull(actualResult);
            var list = actualResult.Value as List<Product>;
            Assert.IsNotNull(list);
            Assert.That(list.Count, Is.EqualTo(3));
        }
    }
}