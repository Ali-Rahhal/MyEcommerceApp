using ECApp.DataAccess.Repository.IRepository;
using ECApp.Models;
using ECApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyEcommerceApp.Areas.Admin.Pages.Products
{
    [Authorize(Roles = SD.Role_Admin)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexModel(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnGet()
        {
        }

        //API CALLS for getting all products in Json format for DataTables
        public JsonResult OnGetAll()//The route is /Admin/Products/Index?handler=All
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();//Include Category data using eager loading
            return new JsonResult(new { data = productList });//We return JsonResult because we will call this method using AJAX
        }

        //API CALL for deleting a product//Didnt use OnPostDelete because it needs the link to send a form and it causes issues with DataTables
        public IActionResult OnGetDelete(int? id)//The route is /Admin/Products/Index?handler=Delete&id=1
        {
            var productToBeDeleted = _unitOfWork.Product.Get(o => o.Id == id);
            if (productToBeDeleted == null)
            {
                return new JsonResult(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                               productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return new JsonResult(new { success = true, message = "Product deleted successfully" });
        }
    }
}