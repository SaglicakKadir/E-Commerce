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

namespace ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SellersController : Controller
    {
        private readonly EcommerceContext _context;
        AuthorizationClass authorization = new AuthorizationClass();

        public SellersController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: Admin/Sellers
        public async Task<IActionResult> Index()
        {
            if (authorization.IsAuthorized("viewSeller", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");//tüm yetkileri işlem yapılacak actionlara yazmalısın
            }

            var ecommerceContext = _context.Sellers.Include(s => s.City).Where(u => u.IsDeleted == false);
            return View(await ecommerceContext.ToListAsync());
        }

        // GET: Admin/Sellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sellers == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .Include(s => s.City)
                .FirstOrDefaultAsync(m => m.SellerId == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // GET: Admin/Sellers/Create
        public IActionResult Create()
        {
            if (authorization.IsAuthorized("createSeller", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");//tüm yetkileri işlem yapılacak actionlara yazmalısın
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            return View();
        }

        // POST: Admin/Sellers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SellerId,SellerName,SellerPhone,SellerEmail,SellerPassword,ConfirmPassword,Banned,SellerDescription,SellerRate,CityId,IsDeleted")] ecommerce.Models.Seller seller)
        {
            SHA256 sHA256;
            byte[] passwordBytes, hashedBytes;
            if (ModelState.IsValid)
            {
                sHA256 = SHA256.Create();
                passwordBytes = Encoding.Unicode.GetBytes(seller.SellerEmail.Trim() + seller.SellerPassword.Trim());
                hashedBytes = sHA256.ComputeHash(passwordBytes);
                seller.SellerPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
                _context.Add(seller);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", seller.CityId);
            return View(seller);
        }

        // GET: Admin/Sellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (authorization.IsAuthorized("editSeller", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");//tüm yetkileri işlem yapılacak actionlara yazmalısın
            }
            if (id == null || _context.Sellers == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", seller.CityId);
            return View(seller);
        }

        // POST: Admin/Sellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SellerId,SellerName,SellerPhone,ConfirmPassword,SellerEmail,SellerPassword,Banned,SellerDescription,SellerRate,CityId,IsDeleted")] ecommerce.Models.Seller seller, string OldPassword, string OriginalPassword)
        {
            SHA256 sHA256;
            byte[] passwordBytes, hashedBytes;
            if (id != seller.SellerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                sHA256 = SHA256.Create();
                passwordBytes = Encoding.Unicode.GetBytes(seller.SellerEmail.Trim() + OldPassword.Trim());
                hashedBytes = sHA256.ComputeHash(passwordBytes);
                if (BitConverter.ToString(hashedBytes).Replace("-", "") == OriginalPassword)
                {

                    passwordBytes = Encoding.Unicode.GetBytes(seller.SellerEmail.Trim() + seller.SellerPassword.Trim());
                    hashedBytes = sHA256.ComputeHash(passwordBytes);
                    seller.SellerPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
                    try
                    {
                        _context.Update(seller);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SellerExists(seller.SellerId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", seller.CityId);
            return View(seller);
        }

        // GET: Admin/Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (authorization.IsAuthorized("deleteSeller", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");//tüm yetkileri işlem yapılacak actionlara yazmalısın
            }
            if (id == null || _context.Sellers == null)
            {
                return NotFound();
            }

            var seller = await _context.Sellers
                .Include(s => s.City)
                .FirstOrDefaultAsync(m => m.SellerId == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // POST: Admin/Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sellers == null)
            {
                return Problem("Entity set 'EcommerceContext.Sellers'  is null.");
            }
            var seller = await _context.Sellers.FindAsync(id);
            if (seller != null)
            {
               seller.IsDeleted= true;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerExists(int id)
        {
          return (_context.Sellers?.Any(e => e.SellerId == id)).GetValueOrDefault();
        }
    }
}
