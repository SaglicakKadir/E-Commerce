#nullable disable
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

namespace ecommerce.Controllers
{
    public class CustomersController : Controller
    {
        private readonly EcommerceContext _context;

        public CustomersController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: Customers


        // GET: Customers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        public IActionResult LogIn(string currentUrl)
        {
            ViewData["currentUrl"] = currentUrl;
            return View();
        }
        public void TransferCart(long customerId, Models.EcommerceContext ecommerceContext, HttpContext httpContext, string newCart = null)
        {

            Models.Product product;
            string[] cartItems;
            string[] itemDetails;
            OrderDetail orderDetail;
            string cartItem;
            CookieOptions cookieOptions = new CookieOptions();

            string cart;
            if (newCart == null)
            {
                cart = Request.Cookies["cart"];
            }
            else
            {
                cart = newCart;
                
            }
            if(cart=="")
            {
                cart = null;
            }
            short productId;
            
            Order order;
            if (httpContext.Session.GetString("order") == null)
            {
                order = new Order();
                order.AllDelivered = false;
                order.IsCart = true;
                order.Cancelled = false;
                order.CustomerId = customerId;
                order.PaymentComplete = false;
                order.TimeStamp = DateTime.Now;
            }
            else
            {
                order = ecommerceContext.Orders.FirstOrDefault(o => o.OrderId == Convert.ToInt64(httpContext.Session.GetString("order")));
            }
            order.OrderDetails = new List<OrderDetail>();
            order.OrderPrice = 0;
            if (cart != null)
            {
                cartItems = cart.Split(',');
                for (short i = 0; i < cartItems.Length; i++)
                {
                    orderDetail = new OrderDetail();
                    cartItem = cartItems[i];
                    itemDetails = cartItem.Split(':');//[0] solu gösterir,[1] sağı gösterir                
                    productId = Convert.ToInt16(itemDetails[0]);
                    product = ecommerceContext.Products.FirstOrDefault(m => m.ProductId == productId);
                    orderDetail.Count = Convert.ToByte(itemDetails[1]);
                    orderDetail.Price = product.ProductPrice * orderDetail.Count;
                    orderDetail.Product = product;
                    order.OrderPrice += orderDetail.Price;
                    order.OrderDetails.Add(orderDetail);
                }
                if (httpContext.Session.GetString("order") == null)
                {
                    ecommerceContext.Add(order);
                    ecommerceContext.SaveChanges();
                    httpContext.Session.SetString("order", order.OrderId.ToString());
                }
                else
                {
                    ecommerceContext.Update(order);
                    ecommerceContext.SaveChanges();
                }
            }
            else
            {
                if (httpContext.Session.GetString("order") != null)
                {
                    ecommerceContext.Remove(order);
                    ecommerceContext.SaveChanges();
                    httpContext.Session.Remove("order");
                }
            }
        }
        public void ProcessLogIn([Bind("CustomerEmail", "CustomerPassword")] Models.Customer customer, string currentUrl)
        {
            SHA256 sHA256;
            byte[] passwordBytes, hashedBytes;

            var dbUser = _context.Customers.FirstOrDefault(m => m.CustomerEmail == customer.CustomerEmail);//veri tabanından gelen emailleri eşleşiyormu diye kontrol edip getiriyor
            if (dbUser != null)
            {
                string loginPassword;
                sHA256 = SHA256.Create();
                passwordBytes = Encoding.Unicode.GetBytes(customer.CustomerEmail.Trim() + customer.CustomerPassword.Trim());
                hashedBytes = sHA256.ComputeHash(passwordBytes);
                loginPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
                if (loginPassword == dbUser.CustomerPassword)
                {
                    this.HttpContext.Session.SetString("customer", dbUser.CustomerId.ToString());//sessiona setring ile bişey oluşturuyoruz
                    TransferCart(dbUser.CustomerId, _context, this.HttpContext);
                    Response.Redirect(currentUrl);
                    return;
                }
            }
            Response.Redirect("/LogIn");
        }
        // GET: Customers/Create
        public IActionResult Create(string currentUrl, bool noPassword = false)
        {
            ViewData["noPassword"] = noPassword;
            ViewData["currentUrl"] = currentUrl;
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,CustomerSurname,CustomerEmail,CustomerPhone,CustomerPassword,ConfirmPassword,CustomerAddress,IsDeleted")] Customer customer, string currentUrl)
        {
            if (ModelState.IsValid)
            {
                SHA256 sHA256;
                byte[] passwordBytes, hashedBytes;
                string loginPassword;
                sHA256 = SHA256.Create();
                passwordBytes = Encoding.Unicode.GetBytes(customer.CustomerEmail.Trim() + customer.CustomerPassword.Trim());
                hashedBytes = sHA256.ComputeHash(passwordBytes);
                loginPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
                customer.CustomerPassword = loginPassword;

                _context.Add(customer);
                await _context.SaveChangesAsync();
                this.HttpContext.Session.SetString("customer", customer.CustomerId.ToString());
                TransferCart(customer.CustomerId, _context, this.HttpContext);
                return Redirect(currentUrl);

            }
            return View(customer);
        }
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CustomerId,CustomerName,CustomerSurname,CustomerEmail,CustomerPhone,CustomerPassword,CustomerAddress,IsDeleted")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }
        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
