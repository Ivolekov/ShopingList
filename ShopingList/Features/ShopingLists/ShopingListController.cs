using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopingList.Data.Models;
using ShopingList.Extentions;
using ShopingList.Features.Products.Services;
using ShopingList.Features.ShopingLists.Models;

namespace ShopingList.Features.ShopingLists
{
    [Authorize]
    public class ShopingListController : Controller
    {
        private readonly IShopingListService shopingListService;
        private readonly IProductService productService;
        private readonly ILogger logger;

        public ShopingListController(IShopingListService shopingListService, IProductService productService, ILogger logger)
        {
            this.shopingListService = shopingListService;
            this.productService = productService;
            this.logger = logger;
        }

        // GET: ShopingListController
        public async Task<IActionResult> Index()
        {
            try
            {
                var groceryLists = await shopingListService.GetAllGroceriesListAsync(User.GetId());
                var groceryListVM = new List<GroceryListVM>();
                foreach (var groceryList in groceryLists)
                {
                    GroceryListVM gLModel = new GroceryListVM
                    {
                        Id = groceryList.Id,
                        Title = groceryList.Title,
                        Product_GroceryList = groceryList.Product_GroceryList
                    };
                    groceryListVM.Add(gLModel);
                }
                return View(groceryListVM);
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
        }

        // GET: ShopingListController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var groceryList = await shopingListService.GetGroceriesListByIdAsync(id);

                if (groceryList == null)
                {
                    return NotFound($"Shoping list do not exists. ID: {id}");
                }

                if (groceryList.UserId != User.GetId())
                {
                    return Unauthorized($"You are not authorize for this operation.");
                }
                GroceryListVM model = new GroceryListVM
                {
                    Id = groceryList.Id,
                    Title = groceryList.Title,
                    Product_GroceryList = await shopingListService.GetProductGroceryListByGLIdAsync(id)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
        }

        // GET: ShopingListController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShopingListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title")] GroceriesListModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var groceriesList = new GroceryList
                    {
                        Title = model.Title,
                        UserId = User.GetId()
                    };
                    await shopingListService.CreateGroceriesListAsync(groceriesList);
                    TempData["AlertMsg"] = $"Shoping list  {groceriesList.Title} was created.";
                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }

        }

        // GET: ShopingListController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var groceryList = await shopingListService.GetGroceriesListByIdAsync(id);

                if (groceryList == null)
                {
                    return NotFound($"Shoping list do not exists. ID: {id}");
                }

                if (groceryList.UserId != User.GetId())
                {
                    return Unauthorized("You are not authorize for this operation.");
                }
                GroceriesListModel model = new GroceriesListModel
                {
                    Id = groceryList.Id,
                    Title = groceryList.Title
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
        }

        // POST: ShopingListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Title, Product_GroceryList")] GroceriesListModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return NotFound($"Shoping list do not exists. ID: {id}");
                }

                if (ModelState.IsValid)
                {
                    GroceryList groceryList = await shopingListService.GetGroceriesListByIdAsync(id);

                    if (groceryList.UserId != User.GetId())
                    {
                        return Unauthorized("You are not authorize for this operation.");
                    }

                    groceryList.Title = model.Title;
                    await shopingListService.UpdateGroceriesListAsync(groceryList);
                    TempData["AlertMsgEdit"] = $"Shoping list {groceryList.Title} was edited.";
                }

            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }

            return View(model);
        }

        // GET: ShopingListController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var groceryList = await shopingListService.GetGroceriesListByIdAsync(id);

                if (groceryList == null)
                {
                    return NotFound($"Shoping list do not exists. ID: {id}");
                }
                if (groceryList.UserId != User.GetId())
                {
                    return Unauthorized("You are not authorize for this operation.");
                }

                GroceriesListModel model = new GroceriesListModel
                {
                    Id = groceryList.Id,
                    Title = groceryList.Title
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
        }

        // POST: ShopingListController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var groceryList = await shopingListService.GetGroceriesListByIdAsync(id);

                if (groceryList == null)
                {
                    return NotFound($"Shoping list do not exists. ID: {id}");
                }
                if (groceryList.UserId != User.GetId())
                {
                    return Unauthorized("You are not authorize for this operation.");
                }

                await shopingListService.DeleteGroceriesListAsync(groceryList);
                TempData["AlertMsg"] = $"Shoping list  {groceryList.Title} was deleted.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToTable(string productName, int groceryListId)
        {
            try
            {
                var product = await productService.GetProductByNameAsync(productName);
                var groceryList = await shopingListService.GetGroceriesListByIdAsync(groceryListId);
                if (product == null)
                {
                    return NotFound($"Product {productName} do not exists.");
                }
                if (groceryList == null)
                {
                    return NotFound($"Shoping list do not exists. ID: {groceryListId}");
                }
                Product_GroceryList productGroceryList = new Product_GroceryList
                {
                    ProductId = product.Id,
                    GroceriesListId = groceryList.Id,
                    Product = product,
                    GroceriesList = groceryList
                };
                var productGroceryListRes = await shopingListService.InsertPrductGroceryListAsync(productGroceryList);

                Product_GroceryListVM pglVM = new Product_GroceryListVM
                {
                    GroceryListId = productGroceryListRes.Id,
                    Product = productGroceryListRes.Product
                };

                return Json(pglVM);
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductGroceryList(int producGroceryListId)
        {
            try
            {
                var productGL = await shopingListService.GetProductGroceryListByIdAsync(producGroceryListId);
                if (productGL == null)
                {
                    return NotFound($"There is not such product in shoping list do not exists. ID: {producGroceryListId}");
                }
                productGL.IsBought = !productGL.IsBought;

                await shopingListService.UpdateProductCroceryListAsync(productGL);
                string markUnmark = productGL.IsBought ? "marked" : "unmarked";
                TempData["AlertMsgRow"] = $"Product {productGL.Product.Name} was {markUnmark}.";

                return Ok(productGL.IsBought);
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductGroceryList(int producGroceryListId)
        {
            try
            {
                var productGL = await shopingListService.GetProductGroceryListByIdAsync(producGroceryListId);

                if (productGL == null)
                {
                    return NotFound($"There is not such product in shoping list do not exists. ID: {producGroceryListId}");
                }

                await shopingListService.DeleteProductCroceryListAsync(productGL);
                TempData["AlertMsgRow"] = $"Product {productGL.Product.Name} was removed.";

                return Ok();
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
        }
    }
}
