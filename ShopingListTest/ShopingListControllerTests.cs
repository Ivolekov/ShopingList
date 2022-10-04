namespace ShopingListTest
{
    public class ShopingListControllerTests
    {
        private static IProductService productService;
        private static IShopingListService shopingListService;
        private static ShopingListController shopingListController;

        [SetUp]
        public void Setup()
        {
            productService = A.Fake<IProductService>();
            shopingListService = A.Fake<IShopingListService>();
            shopingListController = new ShopingListController(shopingListService, productService);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("test-custom-claim", "test claim value"),
           }, "mock"));

            shopingListController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task Index_ShouldReturn_ViewModel_GroceryList()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetAllGroceriesList(A<string>._)).Returns(A.CollectionOfFake<GroceryList>(5));

            //Act
            var response = shopingListController.Index();
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsInstanceOf(typeof(ViewResult), actualResult);
            Assert.IsNotNull(actualResult);
            var model = actualResult.Model as List<GroceryListVM>;
            Assert.IsNotNull(model);
            Assert.That(model.Count, Is.EqualTo(5));
        }

        [Test]
        public void Create_ShoudReturn_View()
        {
            //Act
            var response = shopingListController.Create();
            var actualResult = response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public async Task Create_ShoudReturn_Index()
        {
            //Arrange
            var fakeGroceryList = A.Fake<GroceriesListModel>();

            //Act
            var response = shopingListController.Create(fakeGroceryList);
            var actualResult = await response as RedirectToActionResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Create_RequerdFieldTitle_ShoudReturn_ModelStateError()
        {
            //Arrange
            var groceryList = new GroceriesListModel()
            {
                Id = 1
            };

            string modelStateErrorKey = "Title";
            shopingListController.ModelState.AddModelError(modelStateErrorKey, "The groceries list title should be less than 100 symbols!");

            //Act
            var response = shopingListController.Create(groceryList);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.ViewData);
            Assert.IsNotNull(actualResult.ViewData.ModelState[modelStateErrorKey]);
            var errorsCount = actualResult.ViewData.ModelState[modelStateErrorKey].Errors.Count;
            var errorMsg = actualResult.ViewData.ModelState[modelStateErrorKey].Errors[0].ErrorMessage;
            Assert.That(errorsCount, Is.EqualTo(1));
            Assert.That(errorMsg, Is.EqualTo("The groceries list title should be less than 100 symbols!"));
        }

        [Test]
        public async Task Edit_ShouldReturn_NotFound()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList().Id == 0 ? null : new GroceryList());

            //Act
            var response = shopingListController.Edit(0);
            var actualResult = await response as NotFoundResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task Edit_ShouldReturn_Unauthorized()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList { Id = 1});

            //Act
            var response = shopingListController.Edit(1);
            var actualResult = await response as UnauthorizedResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(401));
        }

        [Test]
        public async Task Edit_ShouldReturn_GroceryList()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList { Id = 1 , UserId = "1"});

            //Act
            var response = shopingListController.Edit(1);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.ViewData.Model);
        }

        [Test]
        public async Task EditGroceryList_ShouldReturn_NotFound()
        {
            //Arrange
            var fakeGroceryList = A.Fake<GroceriesListModel>();

            //Act
            var response = shopingListController.Edit(-1, fakeGroceryList);
            var actualResult = await response as NotFoundResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task EditGroceryList_ShouldReturn_Unauthorized()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList { Id = 1 });
            var groceryList = new GroceriesListModel { Id = 1 };

            //Act
            var response = shopingListController.Edit(1, groceryList);
            var actualResult = await response as UnauthorizedResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(401));
        }
        
        [Test]
        public async Task Edit_RequerdFieldTitle_ShoudReturn_ModelStateError()
        {
            //Arrange
            var griceryList = new GroceriesListModel()
            {
                Id = 1
            };

            string modelStateErrorKey = "Title";
            shopingListController.ModelState.AddModelError(modelStateErrorKey, "Title is required.");

            //Act
            var response = shopingListController.Edit(1, griceryList);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.ViewData);
            Assert.IsNotNull(actualResult.ViewData.ModelState[modelStateErrorKey]);
            var errorsCount = actualResult.ViewData.ModelState[modelStateErrorKey].Errors.Count;
            var errorMsg = actualResult.ViewData.ModelState[modelStateErrorKey].Errors[0].ErrorMessage;
            Assert.That(errorsCount, Is.EqualTo(1));
            Assert.That(errorMsg, Is.EqualTo("Title is required."));
        }

        [Test]
        public async Task EditGroceryList_ShoudReturn_Model()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList { Id = 1, UserId = "1"});
            var griceryList = new GroceriesListModel()
            {
                Id = 1
            };

            //Act
            var response = shopingListController.Edit(1, griceryList);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.Model);
            var model = actualResult.Model as GroceryList;
            Assert.IsNotNull(model);
            Assert.That(model.Id, Is.EqualTo(griceryList.Id));
        }

        [Test]
        public async Task Delete_ShouldReturn_NotFound()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList().Id == 0 ? null : new GroceryList());

            //Act
            var response = shopingListController.Delete(0);
            var actualResult = await response as NotFoundResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task Delete_ShouldReturn_Unauthorized()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList { Id = 1 });

            //Act
            var response = shopingListController.Delete(1);
            var actualResult = await response as UnauthorizedResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(401));
        }

        [Test]
        public async Task DeleteGroceryList_ShoudReturn_Model()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList { Id = 1, UserId = "1" });
            var griceryList = new GroceriesListModel()
            {
                Id = 1
            };

            //Act
            var response = shopingListController.Delete(1);
            var actualResult = await response as ViewResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.Model);
            var model = actualResult.Model as GroceryList;
            Assert.IsNotNull(model);
            Assert.That(model.Id, Is.EqualTo(griceryList.Id));
        }

        [Test]
        public async Task DeleteConfirmed_ShouldReturn_Unauthorized()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList { Id = 1 });

            //Act
            var response = shopingListController.DeleteConfirmed(1);
            var actualResult = await response as UnauthorizedResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(401));
        }

        [Test]
        public async Task DeleteConfirmed_ShouldReturn_Index()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList { Id = 1, UserId = "1" });

            //Act
            var response = shopingListController.DeleteConfirmed(1);
            var actualResult = await response as RedirectToActionResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task AddProductToTable_ShouldReturn_ProductNotFound()
        {
            //Arrange
            A.CallTo(() => productService.GetProductByName(A<string>.Ignored)).Returns(new Product().Id == 0 ? null : new Product());

            //Act
            var response = shopingListController.AddProductToTable(String.Empty, 0);
            var actualResult = await response as NotFoundResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task AddProductToTable_ShouldReturn_GroceryListNotFound()
        {
            //Arrange
            A.CallTo(() => productService.GetProductByName(A<string>.Ignored)).Returns(new Product());
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(new GroceryList().Id == 0 ? null : new GroceryList());

            //Act
            var response = shopingListController.AddProductToTable(String.Empty, 0);
            var actualResult = await response as NotFoundResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task AddProductToTable_ShouldReturn_Json()
        {
            //Arrange
            A.CallTo(() => productService.GetProductByName(A<string>.Ignored)).Returns(A.Fake<Product>());
            A.CallTo(() => shopingListService.GetGroceriesListById(A<int>.Ignored)).Returns(A.Fake<GroceryList>());

            //Act
            var response = shopingListController.AddProductToTable(String.Empty, 0);
            var actualResult = await response as JsonResult;
            
            //Assert
            Assert.IsNotNull(actualResult);
            var list = actualResult.Value as Product_GroceryListVM;
            Assert.IsNotNull(list);
        }

        [Test]
        public async Task UpdateProductGroceryList_ShouldReturn_NotFound()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetProductGroceryListById(A<int>.Ignored)).Returns(new Product_GroceryList().Id == 0 ? null : new Product_GroceryList());

            //Act
            var response = shopingListController.UpdateProductGroceryList(0);
            var actualResult = await response as NotFoundResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task UpdateProductGroceryList_ShouldReturn_Ok()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetProductGroceryListById(A<int>.Ignored)).Returns(A.Fake<Product_GroceryList>());
            var fakeProductGL = A.Fake<Product_GroceryList>();
            A.CallTo(() => shopingListService.UpdateProductCroceryList(fakeProductGL));

            //Act
            var response = shopingListController.UpdateProductGroceryList(0);
            var actualResult = await response as OkObjectResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.Value);
            bool booResult = bool.Parse(actualResult.Value.ToString());
            Assert.IsTrue(booResult);
        }

        [Test]
        public async Task DeleteProductGroceryList_ShouldReturn_NotFound()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetProductGroceryListById(A<int>.Ignored)).Returns(new Product_GroceryList().Id == 0 ? null : new Product_GroceryList());

            //Act
            var response = shopingListController.DeleteProductGroceryList(0);
            var actualResult = await response as NotFoundResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task DeleteProductGroceryList_ShouldReturn_Ok()
        {
            //Arrange
            A.CallTo(() => shopingListService.GetProductGroceryListById(A<int>.Ignored)).Returns(A.Fake<Product_GroceryList>());
            var fakeProductGL = A.Fake<Product_GroceryList>();
            A.CallTo(() => shopingListService.DeleteProductCroceryList(fakeProductGL));

            //Act
            var response = shopingListController.DeleteProductGroceryList(0);
            var actualResult = await response as OkResult;

            //Assert
            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.StatusCode, Is.EqualTo(200));
        }
    }
}
