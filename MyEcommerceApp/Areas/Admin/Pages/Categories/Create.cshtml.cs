using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ECApp.Models;
using ECApp.DataAccess.Repository.IRepository;

namespace MyEcommerceApp.Areas.Admin.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                _unitOfWork.Category.Add(NewCategory);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}