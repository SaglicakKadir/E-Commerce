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
    public class UsersController : Controller
    {
        private readonly UserContext _context;
        AuthorizationClass authorization = new AuthorizationClass();
        public UsersController(UserContext context)
        {
            _context = context;

        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            //if (authorization.IsAuthorized("viewUsers", this.HttpContext.Session) == false) //bu kısmı ilk users create yaparken yorum satırına al
            //{
            //    return Problem("You do not have authorization to view this page");//tüm yetkileri işlem yapılacak actionlara yazmalısın
            //}
            return _context.Users != null ?
                        View(await _context.Users.Where(u => u.IsDeleted == false).ToListAsync()) :
                        Problem("Entity set 'UserContext.Users'  is null.");
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserEmail,UserPassword,ConfirmPassword,IsDeleted,ViewUsers,CreateUser,DeleteUser,EditUser,ViewSellers,CreateSeller,DeleteSeller,EditSeller,ViewCategories,CreateCategory,DeleteCategory,EditCategory,DeleteProduct,EditProduct")] User user)
        {
            SHA256 sHA256;
            byte[] passwordBytes, hashedBytes;



            if (ModelState.IsValid)
            {
                sHA256 = SHA256.Create();
                passwordBytes = Encoding.Unicode.GetBytes(user.UserEmail.Trim() + user.UserPassword.Trim());
                hashedBytes = sHA256.ComputeHash(passwordBytes);
                user.UserPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("UserId,UserEmail,UserPassword,ConfirmPassword,IsDeleted,ViewUsers,CreateUser,DeleteUser,EditUser,ViewSellers,CreateSeller,DeleteSeller,EditSeller,ViewCategories,CreateCategory,DeleteCategory,EditCategory,DeleteProduct,EditProduct")] User user, string OldPassword, string OriginalPassword)
        {

            SHA256 sHA256;
            byte[] passwordBytes, hashedBytes;
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                sHA256 = SHA256.Create();
                passwordBytes = Encoding.Unicode.GetBytes(user.UserEmail.Trim() + OldPassword.Trim());
                hashedBytes = sHA256.ComputeHash(passwordBytes);
                if (BitConverter.ToString(hashedBytes).Replace("-", "") == OriginalPassword)
                {

                    passwordBytes = Encoding.Unicode.GetBytes(user.UserEmail.Trim() + user.UserPassword.Trim());
                    hashedBytes = sHA256.ComputeHash(passwordBytes);
                    user.UserPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.UserId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'UserContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted= true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(short id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
