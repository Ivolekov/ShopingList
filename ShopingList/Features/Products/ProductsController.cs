using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopingList.Data.Models;
using ShopingList.Extentions;
using ShopingList.Features.Products.Models;
using ShopingList.Features.Products.Services;
using System.Configuration;

namespace ShopingList.Features.Products
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        private readonly IConfiguration configuration;

        public ProductsController(IProductService productService, ICategoryService categoryService, ILogger logger, IConfiguration configuration)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.logger = logger;
            this.configuration = configuration;
        }

        // GET: Products
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            try
            {
                var productListRes = await productService.GetAllProductsAsync();
                var productList = new PagedProductVM();
                var pageSize = configuration != null ? configuration.GetValue<int>("PageSize") : 10;
                foreach (var p in productListRes.Skip((currentPage - 1) * pageSize).Take(pageSize))
                {
                    ProductVM productVM = new ProductVM
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Category = p.Category.Name
                    };
                    productList.Products.Add(productVM);
                }

                productList.CurrentPage = currentPage;
                productList.ItemsCount = productListRes.Count();
                productList.PageSize = pageSize;

                return View(productList);
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
                return Redirect("/Error");
            }

        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var categoryList = await categoryService.GetAllProductCategoriesAsync();
                var listItems = categoryList.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).ToList();
                listItems.Insert(0, new SelectListItem() { Value = "-1", Text = Messages.ChooseProductCategory });
                ViewData[Messages.CategoryId] = new SelectList(listItems, "Value", "Text");

                return View();
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage ] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(Messages.LogUsername, this.User.GetUsername()));
                return Redirect("/Error");
            }

        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, CategoryId")] ProductModel model)
        {
            try
            {
                Product product = new Product
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId

                };

                if (await productService.IsProductExistsAsync(product)) 
                {
                    TempData[Messages.AlertMsgError] = string.Format(Messages.ProductExists, model.Name);
                }
                else if (ModelState.IsValid)
                {
                    await productService.CreateProductAsync(product);
                    TempData[Messages.AlertMsg] = string.Format(Messages.ProductAdded, model.Name);
                    return RedirectToAction(nameof(Index));
                }

                var categoryList = await categoryService.GetAllProductCategoriesAsync();
                var listItems = categoryList.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).ToList();
                listItems.Insert(0, new SelectListItem() { Value = "-1", Text = Messages.ChooseProductCategory });
                ViewData[Messages.CategoryId] = new SelectList(listItems, "Value", "Text");
                return View(model);
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage ] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
                return Redirect("/Error");
            }

        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Product product = await productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(string.Format(Messages.ProductNotFound, id));
                }

                var categoryList = await categoryService.GetAllProductCategoriesAsync();
                ViewData[Messages.CategoryId] = new SelectList(categoryList, "Id", "Name");
                ProductVM productVM = new ProductVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    CategoryId = product.Category.Id,
                    Category = product.Category.Name
                };
                return View(productVM);
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage ] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
                return Redirect("/Error");
            }

        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name,CategoryId")] ProductModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return NotFound(string.Format(Messages.ProductNotFound, id));
                }

                if (ModelState.IsValid)
                {
                    Product product = new Product
                    {
                        Id = model.Id,
                        Name = model.Name,
                        CategoryId = model.CategoryId

                    };
                    await productService.UpdateProductAsync(product);
                    TempData[Messages.AlertMsg] = string.Format(Messages.ProductEdited, product.Name);

                    return RedirectToAction(nameof(Index));
                }

                var categoryList = await categoryService.GetAllProductCategoriesAsync();
                ViewData[Messages.CategoryId] = new SelectList(categoryList, "Id", "Name");
                return View(model);
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage ] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
                return Redirect("/Error");
            }
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(string.Format(Messages.ProductNotFound, id));
                }
                var categoryList = await categoryService.GetAllProductCategoriesAsync();
                ViewData[Messages.CategoryId] = new SelectList(categoryList, "Id", "Name");
                return View(product);
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage ] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
                return Redirect("/Error");
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await productService.GetProductByIdAsync(id);
                if (product != null)
                {
                    await productService.DeleteProductAsync(product);
                    TempData[Messages.AlertMsg] = string.Format(Messages.ProductDeleted, product.Name);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[Messages.ExeptionMessage ] = ex.Message;
                TempData[Messages.ExeptionInnerMessage] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format(string.Format(Messages.LogUsername, this.User.GetUsername())));
                return Redirect("/Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetProductsList(string prefix)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(prefix))
                {
                    return Json(null);
                }
                var products = await productService.GetProductsByPrefixAsync(prefix.Trim());
                return Json(products);
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
