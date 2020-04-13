using ASPNET_MVC_EF_CodeFirst.DbContext;
using ASPNET_MVC_EF_CodeFirst.Models;
using ClosedXML.Excel;
using System.IO;
using System.Linq;
using System.Web;
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

        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            // Validate file name
            if (file.FileName.EndsWith("xlsx"))
            {
                var wb = new XLWorkbook(file.InputStream);
                if (wb.TryGetWorksheet("Products", out var wSheet))
                {
                    var allRow = wSheet.Rows();
                    var rowIndex = 0;
                    foreach (var row in allRow)
                    {
                        if (rowIndex >0)
                        {
                            var allCellOfRow = row.Cells().ToArray();
                            var name = allCellOfRow[1].Value.ToString();
                            var price = float.Parse(allCellOfRow[2].Value.ToString());
                            var quantity = int.Parse(allCellOfRow[3].Value.ToString());

                            var newProduct = new Product
                            {
                                Name = name,
                                Price = price,
                                Quantity = quantity
                            };
                            dbContext.Products.Add(newProduct);

                        }
                        rowIndex++;
                    }
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("NotFound");
        }

        [HttpGet]
        public ActionResult Export()
        {
            // Create the workbook and worksheet
            var wb = new XLWorkbook();
            var productSheet = wb.AddWorksheet("Products");
            var currentRow = 1;

            // Add first row header
            productSheet.Cell(currentRow, 1).Value = "Id";
            productSheet.Cell(currentRow, 2).Value = "Name";
            productSheet.Cell(currentRow, 3).Value = "Price";
            productSheet.Cell(currentRow, 4).Value = "Quantity";

            // Get all product from database
            var allProduct = dbContext.Products.ToList();
            foreach (var product in allProduct)
            {
                currentRow++;
                productSheet.Cell(currentRow, 1).Value = product.Id;
                productSheet.Cell(currentRow, 2).Value = product.Name;
                productSheet.Cell(currentRow, 3).Value = product.Price;
                productSheet.Cell(currentRow, 4).Value = product.Quantity;
            }

            // Convert workbook to byte array then response to client
            using (var memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                var byteArrayContent = memoryStream.ToArray();
                var responseResult = File(byteArrayContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "all-product.xlsx");
                return responseResult;
            }
        }

        public HttpStatusCodeResult NotFound()
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
        }
    }
}