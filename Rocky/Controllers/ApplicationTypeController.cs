using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using Rocky.Repository.IRepository;

namespace Rocky.Controllers;

public class ApplicationTypeController:Controller
{
    private readonly IApplicationTypeRepository _db;

    public ApplicationTypeController(IApplicationTypeRepository db)
    {
        _db = db;
    }
    // GET
    public IActionResult Index()
    {
        IEnumerable<ApplicationType> objList = _db.GetAll();
        return View(objList);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ApplicationType obj)
    {
        _db.Add(obj);
        _db.Save();
        return RedirectToAction("Index");
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
    public IActionResult Edit(ApplicationType obj)
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