using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopingList.Data;
using ShopingList.Data.Models;
using ShopingList.Models;
using ShopingList.Services;

namespace ShopingList.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var productListRes = await productService.GetAllProducts();
            var productList = new List<ProductVM>();
            foreach (var p in productListRes)
            {
                ProductVM productVM = new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category.Name
                };
                productList.Add(productVM);
            }            
            return View(productList);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        { 
            var categoryList = await categoryService.GetAllProductCategories();
            var listItems = categoryList.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).ToList();
            listItems.Insert(0, new SelectListItem() { Value = "-1", Text = "Choose product category..." });
            ViewData["CategoryId"] = new SelectList(listItems, "Value", "Text");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, CategoryId")] ProductModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId

                };
                await productService.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            var categoryList = await categoryService.GetAllProductCategories();
            var listItems = categoryList.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).ToList();
            listItems.Insert(0, new SelectListItem() { Value = "-1", Text = "Choose product category..." });
            ViewData["CategoryId"] = new SelectList(listItems, "Value", "Text");
            return View(model);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            var categoryList = await categoryService.GetAllProductCategories();
            ViewData["CategoryId"] = new SelectList(categoryList, "Id", "Name");

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name,CategoryId")] ProductModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Product product = new Product
                    {
                        Id = model.Id,
                        Name = model.Name,
                        CategoryId = model.CategoryId

                    };
                    await productService.UpdateProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var categoryList = await categoryService.GetAllProductCategories();
            ViewData["CategoryId"] = new SelectList(categoryList, "Id", "Name");
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var product = await productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            var categoryList = await categoryService.GetAllProductCategories();
            ViewData["CategoryId"] = new SelectList(categoryList, "Id", "Name");
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await productService.GetProductById(id);
            if (product != null)
            {
                await productService.DeleteProduct(product);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> GetProductsList(string prefix) 
        {
            var products = await productService.GetProductsByPrefix(prefix);
            return Json(products);
        }
    }
}
