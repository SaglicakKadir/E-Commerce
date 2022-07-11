using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ecommerce.Models;

namespace ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly EcommerceContext _context;

        public HomeController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            var ecommerceContext = _context.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Seller).Where(p=>p.IsDeleted==false);
            return View(await ecommerceContext.ToListAsync());
        }
        public async Task<IActionResult> JsonIndex()//androidde json formatında yazmamızı sağlıyor...
        {
            return Json(await _context.Products.ToListAsync());
        }
        // GET: Home/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

       
    }
}
