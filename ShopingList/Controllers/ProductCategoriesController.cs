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
    public class ProductCategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public ProductCategoriesController(ICategoryService categoryService) => this.categoryService = categoryService;

        // GET: ProductCategories
        public async Task<IActionResult> Index()
        {
              return View(await categoryService.GetAllProductCategories());
        }

        // GET: ProductCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] ProductCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                ProductCategory category = new ProductCategory
                {
                    Name = model.Name
                };

                await categoryService.CreateProductCategory(category);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await categoryService.GetProductCategoryById(id);
            if (category == null || id != category.Id)
            {
                return NotFound();
            }
            ProductCategoryModel pcModel = new ProductCategoryModel 
            {
                Id = id,
                Name = category.Name
            };
            return View(pcModel);
        }

        // POST: ProductCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ProductCategoryModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ProductCategory productCategory = new ProductCategory
                    {
                        Id = model.Id,
                        Name = model.Name
                    };
                    await categoryService.UpdateProductCategory(productCategory);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var productCategory = await categoryService.GetProductCategoryById(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCategory = await categoryService.GetProductCategoryById(id);
            if (productCategory != null)
            {
                await categoryService.DeleteProductCategory(productCategory);
            }
            
            return RedirectToAction(nameof(Index));
        }

    }
}
