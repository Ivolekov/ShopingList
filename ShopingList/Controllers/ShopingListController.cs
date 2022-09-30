using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopingList.Data.Models;
using ShopingList.Models;
using ShopingList.Services;

namespace ShopingList.Controllers
{
    public class ShopingListController : Controller
    {
        private readonly IShopingListService shopingListService;

        public ShopingListController(IShopingListService shopingListService) => this.shopingListService = shopingListService;

        // GET: ShopingListController
        public async Task<IActionResult> Index()
        {
            return View(await shopingListService.GetAllShopingList());
        }

        // GET: ShopingListController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var groceriestList = await shopingListService.GetGroceriesListById(id);
            if (groceriestList == null)
            {
                return NotFound();
            }

            return View(groceriestList);
        }

        // GET: ShopingListController/Create
        public async Task<IActionResult> Create()
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
                var groceriesList = new GroceriesList 
                {
                    Title = model.Title
                };
                await shopingListService.CreateGroceriesList(groceriesList);
                return RedirectToAction(nameof(Index));
            }

            return View();

        }

        // GET: ShopingListController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var groceriesList = await shopingListService.GetGroceriesListById(id);

            if (groceriesList == null)
            {
                return NotFound();
            }

            return View(groceriesList);
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
                GroceriesList groceriesList = new GroceriesList
                {
                    Id = model.Id,
                    Title = model.Title
                };
               
                await shopingListService.UpdateGroceriesList(groceriesList);
                return View(groceriesList);
            }

            return View(model);

        }

        // GET: ShopingListController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var groceriesList = await shopingListService.GetGroceriesListById(id);
            
            if (groceriesList == null)
            {
                return NotFound();
            }

            return View(groceriesList);
        }

        // POST: ShopingListController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groceriesList = await shopingListService.GetGroceriesListById(id);
            if (groceriesList != null)
            {
                await shopingListService.DeleteGroceriesList(groceriesList);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
