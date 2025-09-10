using Microsoft.AspNetCore.Mvc.RazorPages;
using MyEcommerceApp.Data;
using MyEcommerceApp.Models;

namespace MyEcommerceApp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Category> objCategoryList { get; set; }

        public void OnGet()
        {
            objCategoryList = _context.Categories.ToList();
        }
    }
}