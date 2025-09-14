using ECApp.DataAccess.Repository.IRepository;
using ECApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyEcommerceApp.Areas.Customer.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Product productforDetails { get; set; }

        public void OnGet(int productId)
        {
            productforDetails = _unitOfWork.Product.Get(o => o.Id == productId, includeProperties: "Category");
        }
    }
}