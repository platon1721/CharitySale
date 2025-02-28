using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Context;
using Domain.Entities;

namespace WebApp.Pages.Transactions
{
    public class EditModel : PageModel
    {
        private readonly DAL.Context.AppDbContext _context;

        public EditModel(DAL.Context.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MoneyTransaction MoneyTransaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneytransaction =  await _context.MoneyTransactions.FirstOrDefaultAsync(m => m.Id == id);
            if (moneytransaction == null)
            {
                return NotFound();
            }
            MoneyTransaction = moneytransaction;
           ViewData["ReceiptId"] = new SelectList(_context.Receipts, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MoneyTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoneyTransactionExists(MoneyTransaction.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MoneyTransactionExists(int id)
        {
            return _context.MoneyTransactions.Any(e => e.Id == id);
        }
    }
}
