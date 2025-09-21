using ECApp.DataAccess.Repository.IRepository;
using ECApp.Models;
using ECApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MyEcommerceApp.Areas.Admin.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string Status { get; set; }

        public void OnGet(string? status)
        {
            Status = status;
        }

        public JsonResult OnGetAll(string status)//The route is /Admin/Orders/Index?handler=All
        {
            IEnumerable<OrderHeader> orderHeaderList;

            //We will show all the orders to admin and employee but only the orders of the logged in user to customer
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaderList = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                orderHeaderList = _unitOfWork.OrderHeader
                    .GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser").ToList();
            }

            switch (status)//We will filter the orders based on the status
            {
                case "pending":
                    orderHeaderList = orderHeaderList.Where(u => u.PaymentStatus == SD.PaymentStatusPending).ToList();
                    break;
                case "inprocess":
                    orderHeaderList = orderHeaderList.Where(u => u.OrderStatus == SD.StatusInProcess).ToList();
                    break;
                case "completed":
                    orderHeaderList = orderHeaderList.Where(u => u.OrderStatus == SD.StatusShipped).ToList();
                    break;
                case "approved":
                    orderHeaderList = orderHeaderList.Where(u => u.OrderStatus == SD.StatusApproved).ToList();
                    break;
                default:
                    break;
            }

            return new JsonResult(new { data = orderHeaderList });//We return JsonResult because we will call this method using AJAX
        }
    }
}
