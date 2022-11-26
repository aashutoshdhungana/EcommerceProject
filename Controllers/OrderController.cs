using ECommerce.Application.Core.Entities;
using EcommerceMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EcommerceMVC.Controllers
{
    [Authorize(Roles = "buyer")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(Guid productId, int numberOfItems)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var product = _appContext.Product.Where(p => p.Id == productId).FirstOrDefault();
            if (product == null) 
            {
                ModelState.AddModelError("Error", "Product not found");
                return View();
            }

            if (product.Quantity < numberOfItems)
            {
                ModelState.AddModelError("Error", "Product not in stock");
                return View();
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
            return RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public IActionResult ViewCart()
        {
            List<Order> orders = _appContext.Order.Where(o => o.UserId == User.Identity.Name && o.Status == "pending").Include(o => o.Product).ToList();
            ViewData["TotalPrice"] = orders.Sum(o => o.TotalPrice);
            return View(orders);
        }

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
            return RedirectToAction("");
        }

        public async Task<IActionResult> CheckOut()
        {
            List<Order> orders = _appContext.Order.Where(o => o.UserId == User.Identity.Name && o.Status == "pending").Include(o => o.Product).ToList();
            double totalPrice = orders.Sum(_ => _.TotalPrice);
            var user = await _userManager.GetUserAsync(User);
            if (totalPrice > user.Wallet)
            {
                ModelState.AddModelError("Error", "Insufficient Balance");
                return View();
            }
            user.Wallet -= totalPrice;
            foreach (var item in orders)
            {
                item.Status = "Success";
                _appContext.Order.Update(item);
            }
            _appContext.SaveChanges();
            return View();
        }

        public async Task<IActionResult> LoadBalance()
        {
            var user = await _userManager.GetUserAsync(User);
            user.Wallet += 10000;
            await _userManager.UpdateAsync(user);
            return Json(new { balance = user.Wallet });
        }

        public async Task<IActionResult> GetBalance()
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(new { balance = user.Wallet });
        }
    }
}
