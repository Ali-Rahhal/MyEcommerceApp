using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyEcommerceApp.Data;
using MyEcommerceApp.Models;

namespace MyEcommerceApp.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category objCategory { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (objCategory.Name == objCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "The Display Order cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _context.Categories.Add(objCategory);
                _context.SaveChanges();
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}