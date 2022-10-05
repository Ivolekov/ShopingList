using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ShopingList.Data;
using ShopingList.Data.Models;
using ShopingList.Features.Products.Models;
using ShopingList.Features.Products.Services;

namespace ShopingList.Features.Products
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
                TempData["AlertMsg"] = $"Category {category.Name} was added.";
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
                return NotFound($"Category do not exists. ID: {id}");
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
                return NotFound($"Category do not exists. ID: {id}");
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
                    TempData["AlertMsg"] = $"Category {productCategory.Name} was edeted.";
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
                return NotFound($"Category do not exists. ID: {id}");
            }

            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCategory = await categoryService.GetProductCategoryById(id);

            if (await categoryService.CheckCategoryCanBeDeleted(id))
            {
                TempData["AlertMsgError"] = $"Тhe category cannot be deleted. There are some products with this category.";
                return RedirectToAction(nameof(Index));
            }
            if (productCategory != null)
            {
                await categoryService.DeleteProductCategory(productCategory);
            }
            TempData["AlertMsg"] = $"Category {productCategory.Name} was deleted.";
            return RedirectToAction(nameof(Index));
        }

    }
}
