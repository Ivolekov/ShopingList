using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopingList.Data.Models;
using ShopingList.Extentions;
using ShopingList.Models;
using ShopingList.Services;

namespace ShopingList.Controllers
{
    [Authorize]
    public class ShopingListController : Controller
    {
        private readonly IShopingListService shopingListService;
        private readonly IProductService productService;

        public ShopingListController(IShopingListService shopingListService, IProductService productService)
        {
            this.shopingListService = shopingListService; 
            this.productService = productService;
        }

        // GET: ShopingListController
        public async Task<IActionResult> Index()
        {
            var groceryLists = await shopingListService.GetAllGroceriesList(this.User.GetId());
            var groceryListVM = new List<GroceryListVM>();
            foreach (var groceryList in groceryLists)
            {
                GroceryListVM gLModel= new GroceryListVM 
                {
                    Id = groceryList.Id,
                    Title = groceryList.Title,
                    Product_GroceryList = groceryList.Product_GroceryList
                };
                groceryListVM.Add(gLModel);
            }
            return View(groceryListVM);
        }

        // GET: ShopingListController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var groceryList = await shopingListService.GetGroceriesListById(id);

            if (groceryList == null)
            {
                return NotFound();
            }

            if (groceryList.UserId != this.User.GetId())
            {
                return Unauthorized(); 
            }

            return View(groceryList);
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
            if (ModelState.IsValid)
            {
                var groceriesList = new GroceryList
                {
                    Title = model.Title,
                    UserId = this.User.GetId()
                };
                await shopingListService.CreateGroceriesList(groceriesList);
                TempData["AlertMsg"] = $"Shoping list  {groceriesList.Title} was created.";
                return RedirectToAction(nameof(Index));
            }

            return View();

        }

        // GET: ShopingListController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var groceryList = await shopingListService.GetGroceriesListById(id);

            if (groceryList == null)
            {
                return NotFound();
            }

            if (groceryList.UserId != this.User.GetId())
            {
                return Unauthorized();
            }

            return View(groceryList);
        }

        // POST: ShopingListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Title, Product_GroceryList")] GroceriesListModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                GroceryList groceryList = await shopingListService.GetGroceriesListById(id);

                if (groceryList.UserId != this.User.GetId())
                {
                    return Unauthorized();
                }

                groceryList.Title = model.Title;
                await shopingListService.UpdateGroceriesList(groceryList);
                TempData["AlertMsgEdit"] = $"Shoping list {groceryList.Title} was edited.";
                return View(groceryList);
            }

            return View(model);
        }

        // GET: ShopingListController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var groceryList = await shopingListService.GetGroceriesListById(id);

            if (groceryList == null)
            {
                return NotFound();
            }
            if (groceryList.UserId != this.User.GetId())
            {
                return Unauthorized();
            }
            return View(groceryList);
        }

        // POST: ShopingListController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groceryList = await shopingListService.GetGroceriesListById(id);

            if (groceryList == null)
            {
                return NotFound();
            }
            if (groceryList.UserId != this.User.GetId())
            {
                return Unauthorized();
            }

            await shopingListService.DeleteGroceriesList(groceryList);
            TempData["AlertMsg"] = $"Shoping list  {groceryList.Title} was deleted.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToTable(string productName, int groceryListId) 
        {
            var product = await productService.GetProductByName(productName);
            var groceryList = await shopingListService.GetGroceriesListById(groceryListId);
            if (product == null)
            {
                return NotFound();
            }
            if (groceryList == null)
            {
                return NotFound();
            }
            Product_GroceryList productGroceryList = new Product_GroceryList 
            {
                ProductId = product.Id,
                GroceriesListId = groceryList.Id,
                Product = product,
                GroceriesList = groceryList
            };
            var productGroceryListRes = await shopingListService.InsertPrductGroceryList(productGroceryList);

            Product_GroceryListVM pglVM = new Product_GroceryListVM 
            {
                GroceryListId = productGroceryListRes.Id,
                Product = productGroceryListRes.Product
            };

            return Json(pglVM);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductGroceryList(int producGroceryListId) 
        {
            var productGL = await shopingListService.GetProductGroceryListById(producGroceryListId);
            if (productGL == null)
            {
                return NotFound();
            }
            productGL.IsBought = !productGL.IsBought;

            await shopingListService.UpdateProductCroceryList(productGL);
            string markUnmark = productGL.IsBought ? "marked" : "unmarked"; 
            TempData["AlertMsgRow"] = $"Product {productGL.Product.Name} was {markUnmark}.";

            return Ok(productGL.IsBought);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductGroceryList(int producGroceryListId)
        {
            var productGL = await shopingListService.GetProductGroceryListById(producGroceryListId);

            if (productGL == null)
            {
                return NotFound();
            }

            await shopingListService.DeleteProductCroceryList(productGL);
            TempData["AlertMsgRow"] = $"Product {productGL.Product.Name} was removed.";

            return Ok();
        }
    }
}
