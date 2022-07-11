using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Controllers
{
    public class CartController : Controller
    {
        string[] cartItems;
        string[] itemDetails;

        string newCart = "";
        string cartItem;
        short totalCount = 0;
        bool itemExist = false;
        CookieOptions cookieOptions = new CookieOptions();


        public struct CartProduct
        {
            public Models.Product Product { get; set; }
            public short Count { get; set; }
            public float Total { get; set; }

        }
        public string Add(long id)
        {
            DbContextOptions<Models.EcommerceContext> options = new DbContextOptions<Models.EcommerceContext>();
            Models.EcommerceContext ecommerceContext = new Models.EcommerceContext(options);
            CustomersController customersController = new CustomersController(ecommerceContext);

            string cart = Request.Cookies["cart"];
            short itemCount = 0;
            if (cart == null)
            {
                newCart = id.ToString() + ":1";
                totalCount = 1;
            }
            else
            {
                //cart = Request.Cookies["cart"];//1:1
                cartItems = cart.Split(',');
                for (short i = 0; i < cartItems.Length; i++)
                {
                    cartItem = cartItems[i];
                    itemDetails = cartItem.Split(':');//[0] solu gösterir,[1] sağı gösterir
                    itemCount = Convert.ToInt16(itemDetails[1]);
                    if (itemDetails[0] == id.ToString())
                    {
                        itemCount++;
                        itemExist = true;
                    }
                    totalCount += itemCount;

                    newCart = newCart + itemDetails[0] + ":" + itemCount.ToString();


                    if (i < cartItems.Length - 1)
                    {
                        newCart = newCart + ",";
                    }
                }
                if (itemExist == false)
                {
                    newCart = newCart + "," + id.ToString() + ":1";
                    totalCount++;
                }
            }
            cookieOptions.Path = "/";
            Response.Cookies.Append("cart", newCart, cookieOptions);

            if (this.HttpContext.Session.GetString("customer") != null)
            {
                customersController.TransferCart(Convert.ToInt64(this.HttpContext.Session.GetString("customer")), ecommerceContext, this.HttpContext, newCart);
            }

            return totalCount.ToString();

        }
        public IActionResult Index()
        {
            DbContextOptions<Models.EcommerceContext> options = new DbContextOptions<Models.EcommerceContext>();
            Models.EcommerceContext ecommerceContext = new Models.EcommerceContext(options);
            Areas.Admin.Controllers.ProductsController productsController = new Areas.Admin.Controllers.ProductsController(ecommerceContext);
            Models.Product product;

            List<CartProduct> cartProducts = new List<CartProduct>();
            string cart = Request.Cookies["cart"];
            float cartTotal = 0;
            short productId;
            if(cart!= null)
            {
                cartItems = cart.Split(',');
                
                for (short i = 0; i < cartItems.Length; i++)
                {
                    cartItem = cartItems[i];
                    itemDetails = cartItem.Split(':');//[0] solu gösterir,[1] sağı gösterir
                    CartProduct cartProduct = new CartProduct();
                    productId = Convert.ToInt16(itemDetails[0]);
                    product = productsController.Product(productId);
                    cartProduct.Product = product;
                    cartProduct.Count = Convert.ToInt16(itemDetails[1]);
                    cartProduct.Total = cartProduct.Count * product.ProductPrice;
                    cartTotal += cartProduct.Total;
                    cartProducts.Add(cartProduct);

                }//FOR İÇERİSİNDE TOTAL HESAPLA VİEW İLE GÖNDER
            }
            

            ViewData["product"] = cartProducts;
            ViewData["cartTotal"] = cartTotal;
            return View();
        }
        public string CalculateTotal(long id, byte count)
        {

            DbContextOptions<Models.EcommerceContext> options = new DbContextOptions<Models.EcommerceContext>();
            Models.EcommerceContext ecommerceContext = new Models.EcommerceContext(options);
            Areas.Admin.Controllers.ProductsController productsController = new Areas.Admin.Controllers.ProductsController(ecommerceContext);
            Models.Product product = productsController.Product(id);
            float subtotal = 0;
            subtotal = product.ProductPrice * count;
            ChangeCookie(id, count);
            return subtotal.ToString();

        }
        private void ChangeCookie(long id, byte count)
        {
            DbContextOptions<Models.EcommerceContext> options = new DbContextOptions<Models.EcommerceContext>();
            Models.EcommerceContext ecommerceContext = new Models.EcommerceContext(options);
            CustomersController customersController = new CustomersController(ecommerceContext);
            
           

            string cart = Request.Cookies["cart"];
            newCart = "";
            Console.WriteLine("input " + cart);
            short itemCount = 0;
            cartItems = cart.Split(',');
            for (short i = 0; i < cartItems.Length; i++)
            {
                cartItem = cartItems[i];
                
                itemDetails = cartItem.Split(':');//[0] solu gösterir,[1] sağı gösterir
                itemCount = Convert.ToInt16(itemDetails[1]);
                if (itemDetails[0] == id.ToString())
                {
                    itemCount = count;
                    
                }
                
                if (itemCount==0)
                {
                    continue;
                }
                totalCount += itemCount;
                newCart = newCart + itemDetails[0] + ":" + itemCount.ToString();
                if (i < cartItems.Length - 1)
                {
                    newCart = newCart + ",";
                }
            }
            if (newCart != "")
            {
                if (newCart.Substring(newCart.Length - 1) == ",")
                {
                    newCart = newCart.Substring(0, newCart.Length - 1);
                    
                }
            }else
            {
                Response.Cookies.Delete("cart");
                return;
            }
            cookieOptions.Path = "/";
            cookieOptions.Expires = DateTime.MaxValue;
            Console.WriteLine("output "+ newCart);
            Response.Cookies.Append("cart", newCart, cookieOptions);
            if(this.HttpContext.Session.GetString("customer") != null)
            {
                customersController.TransferCart(Convert.ToInt64(this.HttpContext.Session.GetString("customer")), ecommerceContext, this.HttpContext, newCart);
            }
           
            
        }
        public void EmptyBasket()
        {
            DbContextOptions<Models.EcommerceContext> options = new DbContextOptions<Models.EcommerceContext>();
            Models.EcommerceContext ecommerceContext = new Models.EcommerceContext(options);
            CustomersController customersController = new CustomersController(ecommerceContext);


            Response.Cookies.Delete("cart");
            if (this.HttpContext.Session.GetString("customer") != null)
            {
                customersController.TransferCart(Convert.ToInt64(this.HttpContext.Session.GetString("customer")), ecommerceContext, this.HttpContext, "");
            }
            Response.Redirect("Index");
        }
    }
}
