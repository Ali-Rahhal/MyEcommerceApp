using ECApp.DataAccess.Repository.IRepository;
using ECApp.Models;
using ECApp.Models.ViewModels;
using ECApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyEcommerceApp.Areas.Admin.Pages.Products
{
    [Authorize(Roles = SD.Role_Admin)]
    public class UpsertModel : PageModel //Upsert : Update + Insert
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public ProductVM VM { get; set; }

        public IActionResult OnGet(int? id)
        {
            VM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            if (id == null || id == 0)
            {
                return Page();
            }
            else
            {
                VM.Product = _unitOfWork.Product.Get(o => o.Id == id);
                if (VM.Product == null)
                {
                    return NotFound();
                }
                return Page();
            }
        }

        public IActionResult OnPost(IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(VM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, VM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    VM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (VM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(VM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(VM.Product);
                }

                _unitOfWork.Save();
                if (VM.Product.Id == 0)
                {
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    TempData["success"] = "Product updated successfully";
                }

                return RedirectToPage("Index");
            }
            else
            {
                // Repopulate CategoryList if ModelState is invalid
                VM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }
            return Page();
        }
    }
}