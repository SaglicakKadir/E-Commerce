using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ecommerce.Models;
using System.Security.Cryptography;
using System.Text;

namespace ecommerce.Areas.Seller.Controllers
{
    [Area("Seller")]
    public class HomeController : Controller
    {
        private readonly EcommerceContext _context;

        public HomeController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: Seller/Home
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult LogIn([Bind("SellerEmail", "SellerPassword")] Models.Seller seller)
        {
            SHA256 sHA256;
            byte[] passwordBytes, hashedBytes;

            var dbUser = _context.Sellers.FirstOrDefault(m => m.SellerEmail == seller.SellerEmail);
            if (dbUser != null)
            {
                string loginPassword;
                sHA256 = SHA256.Create();
                passwordBytes = Encoding.Unicode.GetBytes(seller.SellerEmail.Trim() + seller.SellerPassword.Trim());
                hashedBytes = sHA256.ComputeHash(passwordBytes);
                loginPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
                if (loginPassword == dbUser.SellerPassword)
                {
                    this.HttpContext.Session.SetInt32("merchant", dbUser.SellerId);
                    return RedirectToAction("Index", "Products");
                }
            }
            return RedirectToAction("Index");
        }

           
    }
}




