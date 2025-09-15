using ECApp.DataAccess.Repository.IRepository;
using ECApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyEcommerceApp.Areas.Admin.Pages.Companies
{
    [Authorize(Roles = SD.Role_Admin)]
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

        //API CALLS for getting all companies in Json format for DataTables
        public JsonResult OnGetAll()//The route is /Admin/Companies/Index?handler=All
        {
            var companyList = _unitOfWork.Company.GetAll().ToList();
            return new JsonResult(new { data = companyList });//We return JsonResult because we will call this method using AJAX
        }

        //API CALL for deleting a company//Didnt use OnPostDelete because it needs the link to send a form and it causes issues with DataTables
        public IActionResult OnGetDelete(int? id)//The route is /Admin/Companies/Index?handler=Delete&id=1
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(o => o.Id == id);
            if (companyToBeDeleted == null)
            {
                return new JsonResult(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return new JsonResult(new { success = true, message = "Company deleted successfully" });
        }
    }
}