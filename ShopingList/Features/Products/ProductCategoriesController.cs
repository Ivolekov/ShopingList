using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopingList.Data.Models;
using ShopingList.Extentions;
using ShopingList.Features.Products.Models;
using ShopingList.Features.Products.Services;

namespace ShopingList.Features.Products
{
    [Authorize]
    public class ProductCategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        public ProductCategoriesController(ICategoryService categoryService, ILogger logger)
        {
            this.categoryService = categoryService;
            this.logger = logger;
        }

        // GET: ProductCategories
        public async Task<IActionResult> Index(int pageSize = 10, int currentPage = 1)
        {
            try
            {
                var categoriesRes = await categoryService.GetAllProductCategoriesAsync();
                var categoryList = new PagedProductCategoryVM();

                foreach (var c in categoriesRes.Skip((currentPage - 1) * pageSize).Take(pageSize))
                {
                    ProductCategory category = new ProductCategory
                    {
                        Id = c.Id,
                        Name = c.Name
                    };
                    categoryList.Categories.Add(category);
                }

                categoryList.CurrentPage = currentPage;
                categoryList.ItemsCount = categoriesRes.Count();
                categoryList.PageSize = 10;
                return View(categoryList);
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
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
            try
            {
                if (await categoryService.IsCategoryExistsAsync(model.Name))
                {
                    TempData["AlertMsgError"] = $"Category {model.Name} already exists.";
                }
                else if (ModelState.IsValid)
                {
                    ProductCategory category = new ProductCategory
                    {
                        Name = model.Name
                    };

                    await categoryService.CreateProductCategoryAsync(category);
                    TempData["AlertMsg"] = $"Category {category.Name} was added.";
                    return RedirectToAction(nameof(Index));
                }
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

        // GET: ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await categoryService.GetProductCategoryByIdAsync(id);
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
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
        }

        // POST: ProductCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ProductCategoryModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return NotFound($"Category do not exists. ID: {id}");
                }

                if (ModelState.IsValid)
                {
                    ProductCategory productCategory = new ProductCategory
                    {
                        Id = model.Id,
                        Name = model.Name
                    };
                    await categoryService.UpdateProductCategoryAsync(productCategory);
                    TempData["AlertMsg"] = $"Category {productCategory.Name} was edeted.";

                    return RedirectToAction(nameof(Index));
                }
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

        // GET: ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var productCategory = await categoryService.GetProductCategoryByIdAsync(id);
                if (productCategory == null)
                {
                    return NotFound($"Category do not exists. ID: {id}");
                }

                return View(productCategory);
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var productCategory = await categoryService.GetProductCategoryByIdAsync(id);

                if (await categoryService.CheckCategoryCanBeDeletedAsync(id))
                {
                    TempData["AlertMsgError"] = $"Тhe category cannot be deleted. There are some products with this category.";
                    return RedirectToAction(nameof(Index));
                }
                if (productCategory != null)
                {
                    await categoryService.DeleteProductCategoryAsync(productCategory);
                }
                TempData["AlertMsg"] = $"Category {productCategory.Name} was deleted.";
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
    }
}
