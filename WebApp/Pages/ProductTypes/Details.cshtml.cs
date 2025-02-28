using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using WebApp.Pages.Shared;

namespace WebApp.Pages.ProductTypes
{
    public class DetailsModel : AdminPageModel
    {
        private readonly DAL.Context.AppDbContext _context;

        public DetailsModel(DAL.Context.AppDbContext context)
        {
            _context = context;
        }

        public ProductType ProductType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producttype = await _context.ProductTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (producttype == null)
            {
                return NotFound();
            }
            else
            {
                ProductType = producttype;
            }
            return Page();
        }
    }
}
