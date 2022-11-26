using ECommerce.Application.Core.Entities;
using EcommerceMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _appContext;
        private readonly UserManager<User> _userManager;

        public OrderController(AppDbContext appContext, UserManager<User> userManager)
        {
            _appContext = appContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "buyer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(Guid productId, int numberOfItems)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details","Product", new { Id = productId });
            }
            var product = _appContext.Product.Where(p => p.Id == productId).FirstOrDefault();
            if (product == null) 
            {
                ModelState.AddModelError("Error", "Product not found");
                return RedirectToAction("Details", "Product", new { Id = productId });
            }

            if (product.Quantity < numberOfItems)
            {
                ModelState.AddModelError("Error", "Product not in stock");
                return RedirectToAction("Details", "Product", new { Id = productId });
            }

            var userId = User.Identity.Name;

            Order order = new Order()
            {
                ProductId = productId,
                Quantity = (uint)numberOfItems,
                TotalPrice = product.Price * numberOfItems,
                UserId = userId,
                Status = "pending"
            };
            _appContext.Order.Add(order);
            _appContext.SaveChanges();
            return RedirectToAction("ViewCart", "Order");
        }

        [Authorize(Roles = "buyer")]
        [HttpGet]
        public IActionResult ViewCart()
        {
            List<Order> orders = _appContext.Order.Where(o => o.UserId == User.Identity.Name && o.Status == "pending").Include(o => o.Product).ToList();
            ViewData["TotalPrice"] = orders.Sum(o => o.TotalPrice);
            return View(orders);
        }

        [Authorize(Roles = "buyer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            if (id == null || _appContext.Order == null)
            {
                return NotFound();
            }
            var order = _appContext.Order.Where(o => o.Id == id).FirstOrDefault();
            if (order == null) 
            {
                return NotFound();
            }
            _appContext.Order.Remove(order);
            _appContext.SaveChanges();
            return RedirectToAction("ViewCart", "Order");
        }

        [HttpPost]
        [Authorize(Roles = "buyer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut()
        {
            List<Order> orders = _appContext.Order.Where(o => o.UserId == User.Identity.Name && o.Status == "pending").Include(o => o.Product).ToList();
            double totalPrice = orders.Sum(_ => _.TotalPrice);
            var user = await _userManager.GetUserAsync(User);
            if (totalPrice > user.Wallet)
            {
                ModelState.AddModelError("Error", "Insufficient Balance");
                return View("ViewCart", orders);
            }
            user.Wallet -= totalPrice;
            foreach (var item in orders)
            {
                var seller = _userManager.Users.Where(x => x.Email == item.Product.UserId).FirstOrDefault();
                seller.Wallet += item.TotalPrice;
                await _userManager.UpdateAsync(seller);
                item.Status = "Success";
                _appContext.Order.Update(item);
            }
            _appContext.SaveChanges();
            return View(orders);
        }

        [HttpGet]
        [Authorize(Roles = "buyer")]
        public IActionResult LoadBalance()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "buyer")]

        public async Task<IActionResult> LoadBalance(double amount)
        {
            var user = await _userManager.GetUserAsync(User);
            user.Wallet += amount;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "seller, buyer")]
        public async Task<IActionResult> GetBalance()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new { balance = user.Wallet });
        }
    }
}
