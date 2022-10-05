using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopingList.Data.Models;
using ShopingList.Extentions;
using ShopingList.Features.Products.Models;
using ShopingList.Features.Products.Services;

namespace ShopingList.Features.Products
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;

        public ProductsController(IProductService productService, ICategoryService categoryService, ILogger logger)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.logger = logger;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            try
            {
                var productListRes = await productService.GetAllProductsAsync();
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
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
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
                listItems.Insert(0, new SelectListItem() { Value = "-1", Text = "Choose product category..." });
                ViewData["CategoryId"] = new SelectList(listItems, "Value", "Text");

                return View();
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
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
                if (ModelState.IsValid)
                {
                    Product product = new Product
                    {
                        Name = model.Name,
                        CategoryId = model.CategoryId

                    };
                    await productService.CreateProductAsync(product);
                    TempData["AlertMsg"] = $"Product {product.Name} was added.";
                    return RedirectToAction(nameof(Index));
                }
                var categoryList = await categoryService.GetAllProductCategoriesAsync();
                var listItems = categoryList.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).ToList();
                listItems.Insert(0, new SelectListItem() { Value = "-1", Text = "Choose product category..." });
                ViewData["CategoryId"] = new SelectList(listItems, "Value", "Text");
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

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Product product = await productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product do not exists. ID: {id}");
                }

                var categoryList = await categoryService.GetAllProductCategoriesAsync();
                ViewData["CategoryId"] = new SelectList(categoryList, "Id", "Name");
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
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
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
                    return NotFound($"Product do not exists. ID: {id}");
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
                    TempData["AlertMsg"] = $"Product {product.Name} was edited.";

                    return RedirectToAction(nameof(Index));
                }

                var categoryList = await categoryService.GetAllProductCategoriesAsync();
                ViewData["CategoryId"] = new SelectList(categoryList, "Id", "Name");
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

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product do not exists. ID: {id}");
                }
                var categoryList = await categoryService.GetAllProductCategoriesAsync();
                ViewData["CategoryId"] = new SelectList(categoryList, "Id", "Name");
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
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
                    TempData["AlertMsg"] = $"Product {product.Name} was deleted.";
                }
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
                TempData["ExeptionMessage"] = ex.Message;
                TempData["ExeptionInnerMessage"] = ex.InnerException != null ? ex.InnerException.Message : null;
                logger.Log(LogLevel.Error, ex, string.Format($"Username: {this.User.GetUsername()}"));
                return Redirect("/Error");
            }

        }
    }
}
