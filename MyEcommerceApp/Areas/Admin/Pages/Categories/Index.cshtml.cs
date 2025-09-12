using Microsoft.AspNetCore.Mvc.RazorPages;
using ECApp.Models;
using ECApp.DataAccess.Repository.IRepository;

namespace MyEcommerceApp.Areas.Admin.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Category> CategoryList { get; set; }

        public void OnGet()
        {
            CategoryList = _unitOfWork.Category.GetAll().ToList();
        }
    }
}