using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#region Старый контроллер
/*
public class CartController:Controller
    {
        private IProductRepository repository;
        public CartController(IProductRepository repo)
        {
            repository = repo;
        }

        public RedirectToActionResult AddToCart(string productId,string returnUrl)
        {
            //Можно оказывается ещё проще 
            // Product product = repository.GetById(ProductID);
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToActionResult RemoveFromCart(int productld, string returnUrl)
        {
            Product product = repository.Products
            .FirstOrDefault(p => p.ProductID == productld.ToString());
            if (product != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
        //Теперь, когда метод Index() возвращает объект ViewResult,
        //инфраструктура MVC визуализирует представление и возвращает сгенерированную
        //HTML-разметку.
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewMode
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }
    }
*/
#endregion
namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private Cart cart;

        public CartController(IProductRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }
        //Теперь, когда метод Index() возвращает объект ViewResult,
        //инфраструктура MVC визуализирует представление и возвращает сгенерированную
        //HTML-разметку.
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewMode
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        public RedirectToActionResult AddToCart(string productId, string returnUrl)
        {
            Product product = repository.Products
            .FirstOrDefault(p => p.ProductID == productId);
            if (product!= null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToActionResult RemoveFromCart(string productId, string returnUrl)
        {
            Product product = repository.Products
            .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
