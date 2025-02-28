using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Users
{
    public class DetailsModel : AdminPageModel
    {
        private readonly DAL.Context.AppDbContext _context;

        public DetailsModel(DAL.Context.AppDbContext context)
        {
            _context = context;
        }

        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }
    }
}
