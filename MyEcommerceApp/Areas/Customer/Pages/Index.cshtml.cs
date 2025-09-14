using ECApp.DataAccess.Repository.IRepository;
using ECApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyEcommerceApp.Areas.Customer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Product> productList { get; set; }

        public void OnGet()
        {
            productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
        }
    }
}