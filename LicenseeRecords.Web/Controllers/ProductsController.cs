using LicenseeRecords.Web.Models;
using LicenseeRecords.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LicenseeRecords.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IJsonDataService<Product> _productService;

        public ProductsController(IJsonDataService<Product> productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        public IActionResult Add()
        {
            return View(new Product());
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.ProductId = new Random().Next(1000, 9999); // Generate random ID for the new product
            await _productService.AddAsync(product);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            await _productService.UpdateAsync(product);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}