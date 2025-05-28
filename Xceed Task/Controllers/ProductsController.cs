using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xceed_Task.Models;

namespace Xceed_Task.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(IProductService productService, UserManager<IdentityUser> userManager)
        {
            _productService = productService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var result = await _productService.GetAllProductsAsync(categoryId);
            ViewBag.Categories = await _productService.GetCategoriesSelectListAsync(categoryId);
            ViewBag.SelectedCategory = categoryId;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductTableForAdmin", result);
            }

            return View(result);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _productService.GetProductDetailsAsync(id.Value);
            if (product == null)
                return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = await _productService.GetCategoriesSelectListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                await _productService.CreateProductAsync(product, userId);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = await _productService.GetCategoriesSelectListAsync(product.CategoryId);
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
                return NotFound();

            ViewData["CategoryId"] = await _productService.GetCategoriesSelectListAsync(product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var success = await _productService.UpdateProductAsync(product);
                if (!success) return NotFound();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = await _productService.GetCategoriesSelectListAsync(product.CategoryId);
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> PublicList(int? categoryId)
        {
            var result = await _productService.GetPublicProductsAsync(categoryId);
            ViewBag.Categories = await _productService.GetCategoriesSelectListAsync(categoryId);
            ViewBag.SelectedCategory = categoryId;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductTable", result);
            }

            return View(result);
        }
    }
}
