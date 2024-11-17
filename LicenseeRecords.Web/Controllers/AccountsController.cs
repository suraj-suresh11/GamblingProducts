using LicenseeRecords.Web.Models;
using LicenseeRecords.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LicenseeRecords.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IJsonDataService<Account> _accountService;

        public AccountsController(IJsonDataService<Account> accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAsync();
            return View(accounts);
        }

        public IActionResult Add() => View(new Account());

        [HttpPost]
        public async Task<IActionResult> Add(Account account)
        {
            account.AccountId = new Random().Next(1000, 9999); // Generate random AccountId
            account.ProductLicence = account.ProductLicence ?? new List<ProductLicence>();
            await _accountService.AddAsync(account);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null) return NotFound();
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Account account)
        {
            await _accountService.UpdateAsync(account);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _accountService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ViewProducts(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null || account.ProductLicence == null)
                return NotFound();

            return View("../Products/Index", account.ProductLicence);
        }
    }
}