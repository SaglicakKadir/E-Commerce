using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ecommerce.Areas.Admin.Models;
using System.Security.Cryptography;
using System.Text;
namespace ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserContext _context;

        public HomeController(UserContext context)
        {
            _context = context;
        }

        // GET: Admin/Home
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LogIn([Bind("UserEmail", "UserPassword")] User user)
        {
            SHA256 sHA256;
            byte[] passwordBytes, hashedBytes;

            var dbUser = _context.Users.FirstOrDefault(m => m.UserEmail == user.UserEmail);
            if (dbUser != null)
            {
                string loginPassword;
                sHA256 = SHA256.Create();
                passwordBytes = Encoding.Unicode.GetBytes(user.UserEmail.Trim() + user.UserPassword.Trim());
                hashedBytes = sHA256.ComputeHash(passwordBytes);
                loginPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
                if (loginPassword == dbUser.UserPassword)
                {
                    this.HttpContext.Session.SetString("guest", dbUser.UserId.ToString());
                    this.HttpContext.Session.SetString("viewUsers", dbUser.ViewUsers.ToString());
                    this.HttpContext.Session.SetString("createUser", dbUser.CreateUser.ToString());
                    this.HttpContext.Session.SetString("deleteUser", dbUser.DeleteUser.ToString());
                    this.HttpContext.Session.SetString("editUser", dbUser.EditUser.ToString());
                    this.HttpContext.Session.SetString("viewSeller", dbUser.ViewSellers.ToString());
                    this.HttpContext.Session.SetString("createSeller", dbUser.CreateSeller.ToString());
                    this.HttpContext.Session.SetString("deleteSeller",dbUser.DeleteSeller.ToString());
                    this.HttpContext.Session.SetString("editSeller", dbUser.EditSeller.ToString());
                    this.HttpContext.Session.SetString("viewCategory", dbUser.ViewCategories.ToString());
                    this.HttpContext.Session.SetString("createCategory", dbUser.CreateCategory.ToString());
                    this.HttpContext.Session.SetString("deleteCategory", dbUser.DeleteCategory.ToString());
                    this.HttpContext.Session.SetString("editCategory", dbUser.EditCategory.ToString());
                    this.HttpContext.Session.SetString("deleteProduct", dbUser.DeleteProduct.ToString());
                    this.HttpContext.Session.SetString("editProduct", dbUser.EditProduct.ToString());
                    return RedirectToAction("Index", "Users");//bu ikisi aynı yere gönderiyor istediğini kullan
                //    Response.Redirect("~/Admin/Users/Index");//bu ikisi aynı yere gönderiyor istediğini kullan
                }
               
            }

            return RedirectToAction("Index");
        }


    }
}
