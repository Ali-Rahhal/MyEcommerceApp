using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyEcommerceApp.Data;
using MyEcommerceApp.Models;

namespace MyEcommerceApp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category? CategoryForUpdate { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            CategoryForUpdate = _context.Categories.Find(id);

            if (CategoryForUpdate == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (CategoryForUpdate.Name == CategoryForUpdate.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "The Display Order cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _context.Categories.Update(CategoryForUpdate);
                _context.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}