using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Pages.Shared;

namespace WebApp.Pages.ProductTypes
{
    public class CreateModel : AdminPageModel
    {
        private readonly IProductTypeService _productTypeService;

        public CreateModel(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CreateProductTypeDto ProductType { get; set; } = default!;
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productTypeService.CreateAsync(ProductType);

            return RedirectToPage("./Index");
        }
    }
}
