using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyEcommerceApp.Data;
using MyEcommerceApp.Models;

namespace MyEcommerceApp.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category CategoryForDelete { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.FirstOrDefault(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            else
            {
                CategoryForDelete = category;
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            _context.Categories.Remove(CategoryForDelete);
            _context.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage("Index");
        }
    }
}