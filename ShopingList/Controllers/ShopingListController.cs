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
            return View(await shopingListService.GetAllGroceriesList(this.User.GetId()));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id, Title")] GroceriesListModel model)
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
            if (groceryList.UserId != this.User.GetId())
            {
                return Unauthorized();
            }
            if (groceryList != null)
            {
                await shopingListService.DeleteGroceriesList(groceryList);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(string productName, int id) 
        {
            var product = await productService.GetProductByName(productName);
            var groceryList = await shopingListService.GetGroceriesListById(id);
            if (product == null) 
            {
                return NotFound(product);
            }
            if (groceryList == null)
            {
                return NotFound(groceryList);
            }
            if (groceryList.UserId != this.User.GetId())
            {
                return Unauthorized();
            }

            //groceriestList.ProductList.Add(product);
            await shopingListService.AddProductToList(product, id);
            return PartialView("_GroceriesListProducts", groceryList);
        }
    }
}
