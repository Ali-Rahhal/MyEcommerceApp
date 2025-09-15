using ECApp.DataAccess.Repository.IRepository;
using ECApp.Models;
using ECApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyEcommerceApp.Areas.Admin.Pages.Companies
{
    [Authorize(Roles = SD.Role_Admin)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Company? companyForUpsert { get; set; }

        public IActionResult OnGet(int? id)
        {
            companyForUpsert = new Company();
            if (id == null || id == 0)
            {
                return Page();
            }
            else
            {
                companyForUpsert = _unitOfWork.Company.Get(o => o.Id == id);
                if (companyForUpsert == null)
                {
                    return NotFound();
                }
                return Page();
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (companyForUpsert == null)
                {
                    return NotFound();
                }

                if (companyForUpsert.Id == 0)
                {
                    _unitOfWork.Company.Add(companyForUpsert);
                }
                else
                {
                    _unitOfWork.Company.Update(companyForUpsert);
                }

                _unitOfWork.Save();
                if (companyForUpsert.Id == 0)
                {
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    TempData["success"] = "Company updated successfully";
                }

                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}