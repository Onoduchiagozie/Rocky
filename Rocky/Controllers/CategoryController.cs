using Microsoft.AspNetCore.Mvc;
using Rocky_Utility;
using Rocky.Data;
using Rocky.Models;
using Rocky.Repository.IRepository;

namespace Rocky.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _db;

    public CategoryController(ICategoryRepository db)
    {
        _db = db;
    }
    public IActionResult Index()
    {
        IEnumerable<Category> objList = _db.GetAll();
        return View(objList);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if (ModelState.IsValid)
        {
            _db.Add(obj);
            _db.Save();
            TempData[WC.Success] = "Category Created Succeessful";
            return RedirectToAction("Index");
        }

        TempData[WC.Error] = "Eroor WHile Creating Category";

        return View(obj);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id==0)
        {
            return NotFound();
        }
        var obj = _db.Find(id.GetValueOrDefault());
        return View(obj);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (ModelState.IsValid)
        {
            _db.Update(obj);
            _db.Save();
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id==0)
        {
            return NotFound();
        }
        var obj = _db.Find(id.GetValueOrDefault());
        return View(obj);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var obj = _db.Find(id.GetValueOrDefault());
        _db.Remove(obj); 
        _db.Save();
        return RedirectToAction("Index");
    }
}