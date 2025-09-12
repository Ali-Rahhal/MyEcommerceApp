using Microsoft.AspNetCore.Mvc.RazorPages;
using ECApp.DataAccess.Data;
using ECApp.Models;

namespace MyEcommerceApp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Category> CategoryList { get; set; }

        public void OnGet()
        {
            CategoryList = _context.Categories.ToList();
        }
    }
}