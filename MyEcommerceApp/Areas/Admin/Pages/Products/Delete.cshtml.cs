using ECApp.DataAccess.Repository.IRepository;
using ECApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyEcommerceApp.Areas.Admin.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Product? ProductForDelete { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductForDelete = _unitOfWork.Product.Get(o => o.Id == id);

            if (ProductForDelete == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ProductForDelete == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(ProductForDelete);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToPage("Index");
        }
    }
}