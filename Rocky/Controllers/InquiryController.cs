using Microsoft.AspNetCore.Mvc;
using Rocky_Utility;
using Rocky.Models;
using Rocky.Models.ViewModel;
using Rocky.Repository.IRepository;
using Rocky.Utility;

namespace Rocky.Controllers;

public class InquiryController : Controller
{
    private readonly IInquiryDetailRepository _detailRepository;
    private readonly IInquiryHeaderRepository _headerRepository;
    
    
    [BindProperty]
    public InquiryVm InquiryVm { get; set; }

    public InquiryController(IInquiryDetailRepository detailRepository,
        IInquiryHeaderRepository headerRepository)
    {
        _detailRepository = detailRepository;
        _headerRepository = headerRepository;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }

    
    public IActionResult Details(int id)
    {
        InquiryVm = new InquiryVm()
        {
            InquiryHeader = _headerRepository.FirstOrDefault(u=>
                u.Id==id),
            InquiryDetails = _detailRepository.GetAll(u=>
                u.InquiryHeaderId==id,includeProperties:"Product")
        }; 
        return View(InquiryVm);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Details()
    {
        List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
        InquiryVm.InquiryDetails = _detailRepository.GetAll(u =>
            u.InquiryHeaderId == InquiryVm.InquiryHeader.Id);
        foreach ( var detail in InquiryVm.InquiryDetails)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                ProductId = detail.ProductId
            };
            shoppingCartsList.Add(shoppingCart);
        }
        HttpContext.Session.Clear();
        HttpContext.Session.Set(WC.SessionCart,shoppingCartsList);
        HttpContext.Session.Set(WC.SessionInquiryId,InquiryVm.InquiryHeader.Id);

        return RedirectToAction("Index","Cart");
    }

    [HttpPost]
    public IActionResult Delete()
    {
        InquiryHeader inquiryHeader = _headerRepository
            .FirstOrDefault(u => u.Id == InquiryVm.InquiryHeader.Id);
        IEnumerable<InquiryDetail> inquiryDetails = _detailRepository
            .GetAll(u => u.InquiryHeaderId == InquiryVm.InquiryHeader.Id);
        _detailRepository.RemoveRange(inquiryDetails);
        _headerRepository.Remove(inquiryHeader);
        _headerRepository.Save();
        return RedirectToAction(nameof(Index));
    }
    
    
    
    [HttpGet]
    public IActionResult GetInquiryList()
    {
        return Json(new { data= _headerRepository.GetAll()});
    }
}