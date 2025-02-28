using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using WebApp.Pages.Shared;

namespace WebApp.Pages.ProductTypes
{
    public class EditModel : AdminPageModel
    {
        private readonly DAL.Context.AppDbContext _context;

        public EditModel(DAL.Context.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductType ProductType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producttype =  await _context.ProductTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (producttype == null)
            {
                return NotFound();
            }
            ProductType = producttype;
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ProductType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductTypeExists(ProductType.Id))
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

        private bool ProductTypeExists(int id)
        {
            return _context.ProductTypes.Any(e => e.Id == id);
        }
    }
}
