using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SchoolApplication.Data;
using SchoolApplication.Models;
using SchoolApplication.Models.ViewModels;
using SchoolApplication.Messages;

namespace SchoolApplication.Controllers
{

    [Authorize]
    public class ProductController : Controller
    {
        private readonly ObjectDbContext _objectDbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(ObjectDbContext objectDbContext, IWebHostEnvironment hostingEnvironment)
        {
            _objectDbContext = objectDbContext; 
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Roles ="Registerer")]
        public IActionResult Index()
        {
            List<Product> productslist = _objectDbContext.Products.ToList();
            return View(productslist);
        }
        [Authorize(Roles = "Teacher,Student")]
        public IActionResult Index_Shop()
        {
            List<Product> productslist = _objectDbContext.Products.ToList();
            return View(productslist);
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var webRootPath = _hostingEnvironment.WebRootPath; // Get the wwwroot path
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    var imagePath = Path.Combine(webRootPath, "images", uniqueFileName); // Specify the path in wwwroot

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(stream);
                    }

                    // Create a Product instance and populate it
                    var product = new Product
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Author = model.Author,
                        ISBN = model.ISBN,
                        ListPrice = model.ListPrice,
                        Price = model.Price,
                        Price50 = model.Price50,
                        Price100 =  model.Price100,
                        Quantity = model.Quantity,
                        Lecture = model.Lecture,
                        ImagePath = "/images/" + uniqueFileName // Relative path to the image
                    };

                    try
                    {
                        
                        _objectDbContext.Add(product);
                        _objectDbContext.SaveChanges();

                        TempData["success"] = "Product successfully created.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message.ToString());
                    }

                    
                }
            }

            return View(model);
        }

        //Get Action Method
        public IActionResult Edit(int? id)
        {
            if (id == null) { return NotFound(); }
            var product = _objectDbContext.Products.Find(id);
            if(product == null) { return NotFound(); }

            var model = new UpdateProductViewModel
            {
                ProductId = product.Id,
                Title = product.Title,
                Description = product.Description,
                Author = product.Author,
                ISBN = product.ISBN,
                ListPrice = product.ListPrice,
                Price = product.Price,
                Price50 = product.Price50,
                Price100 = product.Price100,
                Quantity = product.Quantity,
                Lecture = product.Lecture,
                ImagePath = product.ImagePath
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(UpdateProductViewModel model)
        {
            if(ModelState.IsValid) 
            {
                var product = _objectDbContext.Find<Product>(model.ProductId);
                
                if (model.ImageFile != null && model.ImageFile.Length > 0) 
                {
                    var webRootPath = _hostingEnvironment.WebRootPath; // Get the wwwroot path

                    if (!string.IsNullOrEmpty(product.ImagePath))
                    {
                        var oldImagePath = Path.Combine(webRootPath, product.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    var imagePath = Path.Combine(webRootPath, "images", uniqueFileName); // Specify the path in wwwroot

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(stream);
                    }

                    // changes in the product.imagepath

                    product.ImagePath = "/images/" + uniqueFileName;
                    

                }

                //change sin the product entities

                product.Title = model.Title;
                product.Description = model.Description;
                product.ListPrice = model.ListPrice;
                product.Price = model.Price;
                product.Description = model.Description;
                product.Price50 = model.Price50;
                product.Author = model.Author;
                product.Lecture = model.Lecture;
                product.ISBN = model.ISBN;
                product.Price100 = model.Price100;
                product.Quantity = model.Quantity;

                try 
                {
                    _objectDbContext.Update<Product>(product);
                    _objectDbContext.SaveChanges();
                    TempData["success"] = "Product successfully updated.";
                    return RedirectToAction("Index");
                
                }
                catch (Exception ex) 
                {
                    ModelState.AddModelError("UpdateDbError", ex.Message.ToString());
                
                }



            }

            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) { return NotFound(); }
            var product = _objectDbContext.Products.Find(id);
            if (product == null) { return NotFound(); }
            return View(product);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public IActionResult DeletePost(int? id)
        {
            if (id == null) { return NotFound(); }
            var product = _objectDbContext.Products.Find(id);
            if (product == null) { return NotFound(); }


            var webRootPath = _hostingEnvironment.WebRootPath; // Get the wwwroot path
            var oldImagePath = Path.Combine(webRootPath, product.ImagePath.TrimStart('/'));
            
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            try
            {
                _objectDbContext.Products.Remove(product);
                _objectDbContext.SaveChanges();
                TempData["success"] = "Product successfully removed.";
                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message.ToString());
            }
           
        }
    }
}
