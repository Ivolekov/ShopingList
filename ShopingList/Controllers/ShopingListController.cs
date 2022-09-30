using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopingList.Controllers
{
    public class ShopingListController : Controller
    {
        // GET: ShopingListController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ShopingListController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShopingListController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShopingListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShopingListController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShopingListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShopingListController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShopingListController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
