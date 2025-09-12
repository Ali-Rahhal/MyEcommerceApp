using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ECApp.DataAccess.Data;
using ECApp.Models;

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
        public Category NewCategory { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (NewCategory.Name == NewCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "The Display Order cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _context.Categories.Add(NewCategory);
                _context.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}