using Antlr.Runtime.Misc;
using ASPNET_MVC_EF_CodeFirst.DbContext;
using ASPNET_MVC_EF_CodeFirst.Models;
using System.Linq; 
using System.Web.Mvc;

namespace ASPNET_MVC_EF_CodeFirst.Controllers
{
    public class ProductController : Controller
    {
        private AssignmentDbContext dbContext;

        public ProductController()
        {
            dbContext = new AssignmentDbContext();
        }
         
        public ActionResult Index()
        {
            var allProduct = dbContext.Products.ToList(); 
            return View(allProduct);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var foundProduct = dbContext.Products.Where(p => p.Id == id).FirstOrDefault();
            if (foundProduct == null)
            {
                return RedirectToAction("NotFound");
            }
            return View(foundProduct);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var foundProduct = dbContext.Products.Where(p => p.Id == id).FirstOrDefault();
            if (foundProduct == null)
            {
                return RedirectToAction("NotFound");
            }
            return View(foundProduct);
        }

        [HttpGet] // Default
        public ActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            var name = formCollection.Get("Name");
            var quantity = int.Parse(formCollection.Get("Quantity"));
            var price = float.Parse(formCollection.Get("Price"));

            var newProduct = new Product 
            {
                Name = name,
                Quantity = quantity,
                Price = price
            };

            dbContext.Products.Add(newProduct);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            var foundProduct = dbContext.Products.Where(p => p.Id == id).FirstOrDefault();

            var name = formCollection.Get("Name");
            var quantity = int.Parse(formCollection.Get("Quantity"));
            var price = float.Parse(formCollection.Get("Price"));

            foundProduct.Name = name;
            foundProduct.Quantity = quantity;
            foundProduct.Price = price;
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var foundProduct = dbContext.Products.Where(p => p.Id == id).FirstOrDefault();
            return View(foundProduct);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var foundProduct = dbContext.Products.Where(p => p.Id == id).FirstOrDefault();
            dbContext.Products.Remove(foundProduct);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public HttpStatusCodeResult NotFound()
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
        }
    }
}