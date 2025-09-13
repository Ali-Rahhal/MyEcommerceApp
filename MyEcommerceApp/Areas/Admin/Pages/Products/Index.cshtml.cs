using ECApp.DataAccess.Repository.IRepository;
using ECApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyEcommerceApp.Areas.Admin.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        public void OnGet()
        {
        }

        //API CALLS for getting all products in Json format for DataTables
        public JsonResult OnGetAll()//The route is /Admin/Products/Index?handler=All
        {
            var productList = _unitOfWork.Product.GetAll("Category").ToList();
            return new JsonResult(new { data = productList });
        }
    }
}