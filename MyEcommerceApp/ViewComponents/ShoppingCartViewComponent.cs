using ECApp.DataAccess.Repository.IRepository;
using ECApp.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyEcommerceApp.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent// The View of this component is located at /Views/Shared/Components/ShoppingCart/Default.cshtml
                                                          // Note that the view must be named Default.cshtml and inside the same structure of folders above
                                                          // with the last folder named after the ViewComponent class without the "ViewComponent" suffix.
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // If the user is logged in, get the cart count from the database
            if (claim != null)
            {
                if(HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    int cartCountAfterLogin = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claim.Value).Count();
                    HttpContext.Session.SetInt32(SD.SessionCart, cartCountAfterLogin);
                }
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }// If the user is not logged in, return a cart count of 0 and clear the session
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
