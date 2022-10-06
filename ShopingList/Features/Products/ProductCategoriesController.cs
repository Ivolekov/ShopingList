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
        private readonly IConfiguration configuration;
        public ProductCategoriesController(ICategoryService categoryService, ILogger logger, IConfiguration configuration)
        {
            this.categoryService = categoryService;
            this.logger = logger;
            this.configuration = configuration;
        }

        // GET: ProductCategories
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            try
            {
                var categoriesRes = await categoryService.GetAllProductCategoriesAsync();
                var categoryList = new PagedProductCategoryVM();
                var pageSize = configuration != null ? configuration.GetValue<int>("PageSize") : 10;
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
                categoryList.PageSize = pageSize;
                return View(categoryList);
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
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
                    TempData[Messages.AlertMsgError] = string.Format(Messages.CategoryExists);
                }
                else if (ModelState.IsValid)
                {
                    ProductCategory category = new ProductCategory
                    {
                        Name = model.Name
                    };

                    await categoryService.CreateProductCategoryAsync(category);
                    TempData[Messages.AlertMsg] = $"Category {category.Name} was added.";
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
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
                    return NotFound(string.Format(Messages.CategoryNotFound, id));
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
                TempData[Messages.ExeptionMessage] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
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
                    return NotFound(string.Format(Messages.CategoryNotFound, id));
                }

                if (ModelState.IsValid)
                {
                    ProductCategory productCategory = new ProductCategory
                    {
                        Id = model.Id,
                        Name = model.Name
                    };
                    await categoryService.UpdateProductCategoryAsync(productCategory);
                    TempData[Messages.AlertMsg] = string.Format(Messages.CategoryEdited, productCategory.Name);

                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
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
                    return NotFound(string.Format(Messages.CategoryNotFound, id));
                }

                return View(productCategory);
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
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
                    TempData[Messages.AlertMsgError] = Messages.CategoryCantDeleted;
                    return RedirectToAction(nameof(Index));
                }
                if (productCategory != null)
                {
                    await categoryService.DeleteProductCategoryAsync(productCategory);
                    TempData[Messages.AlertMsg] = string.Format(Messages.CategoryDeleted, productCategory.Name);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
                return Redirect("/Error");
            }
        }
    }
}
