using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Context;
using Domain.Entities;

namespace WebApp.Pages.Transactions
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.Context.AppDbContext _context;

        public DeleteModel(DAL.Context.AppDbContext context)
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

            var moneytransaction = await _context.MoneyTransactions.FirstOrDefaultAsync(m => m.Id == id);

            if (moneytransaction == null)
            {
                return NotFound();
            }
            else
            {
                MoneyTransaction = moneytransaction;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneytransaction = await _context.MoneyTransactions.FindAsync(id);
            if (moneytransaction != null)
            {
                MoneyTransaction = moneytransaction;
                _context.MoneyTransactions.Remove(MoneyTransaction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
