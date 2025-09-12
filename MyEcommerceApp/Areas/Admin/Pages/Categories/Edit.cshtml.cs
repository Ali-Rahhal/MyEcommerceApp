using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ECApp.DataAccess.Data;
using ECApp.Models;
using ECApp.DataAccess.Repository.IRepository;

namespace MyEcommerceApp.Areas.Admin.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Category? CategoryForUpdate { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            CategoryForUpdate = _unitOfWork.Category.Get(o => o.Id == id);

            if (CategoryForUpdate == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (CategoryForUpdate == null)
            {
                return NotFound();
            }

            if (CategoryForUpdate.Name == CategoryForUpdate.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "The Display Order cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(CategoryForUpdate);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}