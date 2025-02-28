using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
