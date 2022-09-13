using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky_Utility;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModel;
using Rocky.Repository.IRepository;

namespace Rocky.Controllers;

[Authorize(Roles = WC.AdminRole)]
public class ProductController : Controller
{
    private readonly IProductRepository _db;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IProductRepository db,IWebHostEnvironment webHostEnvironment)
    {
        _db = db;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        IEnumerable<Product> objList = _db.GetAll(includeProperties:"Category,ApplicationType"); 
        return View(objList);
    }
    public IActionResult Upsert(int? id)
    {
        var productVm = new ProductVM()
        {
            CategorySelectList =_db.getAllDropDownCategories(),
            ApplicationTypes = _db.getAllDropDownApplicationTypes(),
            Product = new Product(),
        };
        if (id ==null)
        {
            return View(productVm);
        }
        else
        {
            productVm.Product = _db.Find(id.GetValueOrDefault());
            if (productVm.Product == null)
            {
                return NotFound();
            }
            return View(productVm);
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj)
    {
  
            var files = HttpContext.Request.Form.Files;
            string webrootPath = _webHostEnvironment.WebRootPath;

            if (obj.Product.Id == 0)
            {
                string upload = webrootPath + WC.ImagePath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                using (var fileStream= new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                obj.Product.Image = fileName + extension;
                _db.Add(obj.Product);
            }
            else
            {
                var objFromDb = _db.FirstOrDefault(c => obj.Product.Id == c.Id,isTracking:false);
                if (files.Count > 0)    
                {
                    string upload = webrootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    var oldFile = Path.Combine(upload, objFromDb.Image);
                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                    using (var fileStream= new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }   
                    obj.Product.Image = fileName + extension;

                }
                else
                {
                    obj.Product.Image = objFromDb.Image;
                }

                _db.Update(obj.Product);

            }
            _db.Save();
            return RedirectToAction("Index");
      

        return View(obj);
    }
    
    public IActionResult Delete(int id)
    {
        if (id == null || id==0)
        {
            return NotFound();
        }

        var obj = _db.FirstOrDefault(u => u.Id == id, includeProperties: "Category,ApplicationType");
        if(obj==null)
        {
            return NotFound();
        }
        return View(obj);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var obj = _db.Find(id.GetValueOrDefault());
        if (obj == null)
        {
            return NotFound();
        }
        string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;

        var oldFile = Path.Combine(upload, obj.Image);
        if (System.IO.File.Exists(oldFile))
        {
            System.IO.File.Delete(oldFile);
        }
        
        _db.Remove(obj); 
        _db.Save();
        return RedirectToAction("Index");
    }
}