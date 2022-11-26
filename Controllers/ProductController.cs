using ECommerce.Application.Core.Entities;
using EcommerceMVC.Data;
using EcommerceMVC.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace EcommerceMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public ProductController(AppDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Product
        public async Task<IActionResult> Index(string? category = null)
        {
            var appDbContext = _context.Product;
            var categoryList = _context.Product.Select(p => p.Type).Distinct().ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (string categoryItem in categoryList)
            {
                list.Add(new SelectListItem()
                {
                    Text = categoryItem,
                    Value = categoryItem
                });
            }
            ViewData["categoryList"] = list;
            var products = await appDbContext.ToListAsync();
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Type == category).ToList();
            }
            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        [Authorize(Roles = "seller")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Email");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Create([Bind("ProductName,Type,Quantity,Price,Description,DeliveryDays,UserId,Id")] Product product, IFormFile? imageFile = null)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                string imagePath = string.Empty;
                if (imageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(ms);
                        imagePath = ImageFunctions.SaveImage(ms.ToArray(), _environment.WebRootPath);
                    }
                }
                else
                {
                    imagePath = "/Images/dummy.jpg";
                }
                product.Image = "/Images/" + imagePath;
                product.UserId = User?.Identity?.Name;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductName,Type,Quantity,Price,Image,Description,DeliveryDays,UserId,Id")] Product product, IFormFile? imageFile = null)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string imagePath = string.Empty;
                if (imageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(ms);
                        imagePath = ImageFunctions.SaveImage(ms.ToArray(), _environment.WebRootPath);
                    }
                }
                if (!string.IsNullOrEmpty(imagePath))
                {
                    product.Image = imagePath;
                }
                try
                {
                    product.UserId = User.Identity.Name;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'AppDbContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
