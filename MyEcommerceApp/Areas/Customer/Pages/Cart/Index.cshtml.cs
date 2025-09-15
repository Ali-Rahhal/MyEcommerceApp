using ECApp.DataAccess.Repository.IRepository;
using ECApp.Models;
using ECApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MyEcommerceApp.Areas.Customer.Pages.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public void OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;// Get the user's identity. Explanation: User.Identity contains
                                                               // information about the currently logged-in user.
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;// Get the user's ID. Explanation: ClaimTypes.NameIdentifier is a
                                                                                   // standard claim type that represents the unique identifier of the user.

            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == userId,
                includeProperties: "Product"),
            };
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBaseOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }
        }

        public IActionResult OnGetPlus(int id)
        {
            var cart = _unitOfWork.ShoppingCart.Get(s => s.Id == id);
            cart.Count += 1;
            _unitOfWork.ShoppingCart.Update(cart);
            _unitOfWork.Save();
            return RedirectToPage("Index");
        }

        public IActionResult OnGetMinus(int id)
        {
            var cart = _unitOfWork.ShoppingCart.Get(s => s.Id == id);
            if (cart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
                cart.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cart);
            }
            _unitOfWork.Save();
            return RedirectToPage("Index");
        }

        public IActionResult OnGetRemove(int id)
        {
            var cart = _unitOfWork.ShoppingCart.Get(s => s.Id == id);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToPage("Index");
        }

        private double GetPriceBaseOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }
    }
}