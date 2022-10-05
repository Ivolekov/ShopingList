﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using ShopingList.Features.Products.Models;
using ShopingList.Features.Products.Services;

namespace ShopingListTest
{
    public class ProductCategoriesControllerTests
    {
        private static ICategoryService categoryService;
        private static ProductCategoriesController productsCategoriesController;
        private static ILogger logger;

        [SetUp]
        public void Setup()
        {
            categoryService = A.Fake<ICategoryService>();
            logger = A.Fake<ILogger>();
            productsCategoriesController = new ProductCategoriesController(categoryService, logger);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Name, "TestUser"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim("test-custom-claim", "test claim value"),
               }, "mock"));

            productsCategoriesController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task Index_ShouldReturn_ViewModel_ProductsList()
        {
            //Arrange
            A.CallTo(() => categoryService.GetAllProductCategoriesAsync()).Returns(A.CollectionOfFake<ProductCategory>(5));

            //Act
            var response = productsCategoriesController.Index();
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsInstanceOf(typeof(ViewResult), actualResult);
            Assert.IsNotNull(actualResult);
            var model = actualResult.Model as List<ProductCategory>;
            Assert.IsNotNull(model);
            Assert.That(model.Count, Is.EqualTo(5));
        }

        [Test]
        public void Create_ShoudReturn_View()
        {
            //Act
            var response = productsCategoriesController.Create();
            var actualResult = response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public async Task Create_ShoudReturn_Index()
        {
            //Arrange
            var fakeCategory = A.Fake<ProductCategoryModel>();
            productsCategoriesController.TempData = new TempDataDictionary(A.Fake<HttpContext>(), A.Fake<ITempDataProvider>());

            //Act
            var response = productsCategoriesController.Create(fakeCategory);
            var actualResult = await response as RedirectToActionResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Create_RequerdFieldName_ShoudReturn_ModelStateError()
        {
            //Arrange
            var category = new ProductCategoryModel()
            {
                Id = 1
            };

            string modelStateErrorKey = "Name";
            productsCategoriesController.ModelState.AddModelError(modelStateErrorKey, "Name is required.");

            //Act
            var response = productsCategoriesController.Create(category);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.ViewData);
            Assert.IsNotNull(actualResult.ViewData.ModelState[modelStateErrorKey]);
            var errorsCount = actualResult.ViewData.ModelState[modelStateErrorKey].Errors.Count;
            var errorMsg = actualResult.ViewData.ModelState[modelStateErrorKey].Errors[0].ErrorMessage;
            Assert.That(errorsCount, Is.EqualTo(1));
            Assert.That(errorMsg, Is.EqualTo("Name is required."));
        }

        [Test]
        public async Task Edit_ShouldReturn_NotFound()
        {
            //Arrange
            A.CallTo(() => categoryService.GetProductCategoryByIdAsync(A<int>.Ignored)).Returns(new ProductCategory().Id == 0 ? null : new ProductCategory());

            //Act
            var response = productsCategoriesController.Edit(0);
            var actualResult = await response as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task Edit_ShouldReturn_ProductCategory()
        {
            //Arrange
            A.CallTo(() => categoryService.GetProductCategoryByIdAsync(A<int>.Ignored)).Returns(A.Fake<ProductCategory>());

            //Act
            var response = productsCategoriesController.Edit(0);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.ViewData.Model);
        }

        [Test]
        public async Task EditProductCategory_ShouldReturn_NotFound()
        {
            //Arrange
            var fakeProductCategory = A.Fake<ProductCategoryModel>();

            //Act
            var response = productsCategoriesController.Edit(-1, fakeProductCategory);
            var actualResult = await response as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task Edit_RequerdFieldName_ShoudReturn_ModelStateError()
        {
            //Arrange
            A.CallTo(() => categoryService.GetAllProductCategoriesAsync()).Returns(A.CollectionOfFake<ProductCategory>(5));

            var productCategory = new ProductCategoryModel()
            {
                Id = 1
            };

            string modelStateErrorKey = "Name";
            productsCategoriesController.ModelState.AddModelError(modelStateErrorKey, "Name is required.");

            //Act
            var response = productsCategoriesController.Edit(1, productCategory);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.ViewData);
            Assert.IsNotNull(actualResult.ViewData.ModelState[modelStateErrorKey]);
            var errorsCount = actualResult.ViewData.ModelState[modelStateErrorKey].Errors.Count;
            Assert.That(errorsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Delete_ShouldReturn_NotFound()
        {
            //Arrange
            A.CallTo(() => categoryService.GetProductCategoryByIdAsync(A<int>.Ignored)).Returns(new ProductCategory().Id == 0 ? null : new ProductCategory());

            //Act
            var response = productsCategoriesController.Delete(0);
            var actualResult = await response as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task Delete_ShouldReturn_Product_And_CategoriesList()
        {
            //Arrange
            A.CallTo(() => categoryService.GetProductCategoryByIdAsync(A<int>.Ignored)).Returns(A.Fake<ProductCategory>());

            //Act
            var response = productsCategoriesController.Delete(0);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.ViewData.Model);
        }

        [Test]
        public async Task DeleteConfirm_ShoudReturn_Index()
        {
            //Arrange
            A.CallTo(() => categoryService.GetProductCategoryByIdAsync(A<int>.Ignored)).Returns(A.Fake<ProductCategory>());
            productsCategoriesController.TempData = new TempDataDictionary(A.Fake<HttpContext>(), A.Fake<ITempDataProvider>());

            //Act
            var response = productsCategoriesController.DeleteConfirmed(1);
            var actualResult = await response as RedirectToActionResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.ActionName, Is.EqualTo("Index"));
        }
    }
}
