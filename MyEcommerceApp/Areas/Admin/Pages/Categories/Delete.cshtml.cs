using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ECApp.Models;
using ECApp.DataAccess.Repository.IRepository;

namespace MyEcommerceApp.Areas.Admin.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Category? CategoryForDelete { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CategoryForDelete = _unitOfWork.Category.Get(o => o.Id == id);

            if (CategoryForDelete == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (CategoryForDelete == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(CategoryForDelete);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage("Index");
        }
    }
}