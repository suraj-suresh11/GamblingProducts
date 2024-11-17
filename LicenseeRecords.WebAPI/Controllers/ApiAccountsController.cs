using Microsoft.AspNetCore.Mvc;
using LicenseeRecords.WebAPI.Models;
using LicenseeRecords.WebAPI.Services;

namespace LicenseeRecords.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAccountsController : ControllerBase
    {
        private readonly IJsonDataService<Account> _accountService;

        public ApiAccountsController(IJsonDataService<Account> accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Account account)
        {
            account.Id = new Random().Next(1000, 9999);
            await _accountService.AddAsync(account);
            return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Account account)
        {
            if (id != account.Id) return BadRequest();
            await _accountService.UpdateAsync(account);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _accountService.DeleteAsync(id);
            return NoContent();
        }
    }
}