using ECApp.DataAccess.Repository.IRepository;
using ECApp.Models;
using ECApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

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

        [BindProperty]
        public ShoppingCart ShoppingCartForUpsert { get; set; }

        public void OnGet(int productId)
        {
            productforDetails = _unitOfWork.Product.Get(o => o.Id == productId, includeProperties: "Category");
            ShoppingCartForUpsert = new ShoppingCart()
            {
                Count = 1,
                ProductId = productId,
                Product = productforDetails
            };
        }

        public IActionResult OnPost()
        {
            // Check if the user is authenticated// Only Logged-in users can add items to the cart
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;// Get the user's identity. Explanation: User.Identity contains information about the currently logged-in user.
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;// Get the user's ID. Explanation: ClaimTypes.NameIdentifier is a standard claim type that represents the unique identifier of the user.

            ShoppingCartForUpsert.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(s => s.ApplicationUserId == userId && s.ProductId == ShoppingCartForUpsert.ProductId);
            if (ModelState.IsValid)
            {
                if (cartFromDb != null)
                {
                    //shopping cart exists for this user and product
                    cartFromDb.Count += ShoppingCartForUpsert.Count;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);
                    _unitOfWork.Save();
                }
                else
                {
                    //add cart record
                    _unitOfWork.ShoppingCart.Add(ShoppingCartForUpsert);
                    _unitOfWork.Save();// Save changes to the database before updating the session because the cart count depends on the saved data
                    //update session after adding item to cart
                    int cartCountAfterAdding = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == userId).Count();
                    HttpContext.Session.SetInt32(SD.SessionCart, cartCountAfterAdding);

                }
                TempData["success"] = "Item added to cart successfully";
                

                return RedirectToPage("/Index");
            }
            else
            {
                productforDetails = _unitOfWork.Product.Get(o => o.Id == ShoppingCartForUpsert.ProductId, includeProperties: "Category");// Re-fetch product details if the model state is invalid
                return Page();
            }
        }
    }
}