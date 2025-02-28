using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Context;
using Domain.Entities;

namespace WebApp.Pages.Transactions
{
    public class CreateModel : PageModel
    {
        private readonly DAL.Context.AppDbContext _context;

        public CreateModel(DAL.Context.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public MoneyTransaction MoneyTransaction { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MoneyTransactions.Add(MoneyTransaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
