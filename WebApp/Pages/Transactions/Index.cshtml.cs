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
    public class IndexModel : PageModel
    {
        private readonly DAL.Context.AppDbContext _context;

        public IndexModel(DAL.Context.AppDbContext context)
        {
            _context = context;
        }

        public IList<MoneyTransaction> MoneyTransaction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            MoneyTransaction = await _context.MoneyTransactions
                .Include(m => m.Receipt).ToListAsync();
        }
    }
}
