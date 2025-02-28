using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Context;
using Domain.Entities;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Products
{
    public class DetailsModel : AdminPageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }
    }
}
