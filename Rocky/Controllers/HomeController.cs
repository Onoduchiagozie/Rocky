using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocky_Utility;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModel;
using Rocky.Repository.IRepository;
using Rocky.Utility;

namespace Rocky.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public HomeController(ILogger<HomeController> logger,IProductRepository productRepository,ICategoryRepository categoryRepository)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public IActionResult Index()
    {
        HomeVM homeVm = new HomeVM()
        {
            Product = _productRepository.GetAll(includeProperties:"Category,ApplicationType"),
            Categorie = _categoryRepository.GetAll()
        };
        return View(homeVm);
    }
    
    public IActionResult Details(int id)
    {
        List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
        if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
            HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
        {
            shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
        }  
        
        DetailsVm detailsVm = new DetailsVm()
        { 
            Product = _productRepository.FirstOrDefault(u=>u.Id==id,includeProperties:"Category,ApplicationType"),
            ExistsInCart = false
        };

        foreach (var item in shoppingCartsList)
        {
            if (item.ProductId == id)
            {
                detailsVm.ExistsInCart = true;
            }
        }
        return View(detailsVm);
    }

    public IActionResult RemoveFromCart(int id)
    {
        List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
        if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
            HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
        {
            shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
        }

        var ItemToRemove = shoppingCartsList.SingleOrDefault(r => r.ProductId == id);
        if (ItemToRemove != null)
        {
            shoppingCartsList.Remove(ItemToRemove);
        }
        HttpContext.Session.Set(WC.SessionCart,shoppingCartsList);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost,ActionName("Details")]
    public IActionResult DetailsPost(int id,DetailsVm detailsVm)
    {
        List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
        if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
            HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
        {
            shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
        }   
        shoppingCartsList.Add( new ShoppingCart{ ProductId = id,SqFt =detailsVm.Product.TempSqFt });
        HttpContext.Session.Set(WC.SessionCart,shoppingCartsList);
        return RedirectToAction(nameof(Index));
    }
    
    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None, 
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult Privacy()
    {
        return View();
    }
    

}