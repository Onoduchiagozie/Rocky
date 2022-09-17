using System.Security.Claims;
using System.Text;
using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky_Utility;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModel;
using Rocky.Repository.IRepository;
using Rocky.Utility;

namespace Rocky.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IApplicationUserRepository _userRepo;
    private readonly IProductRepository _productRepository;
    private readonly IInquiryDetailRepository _detailRepository;
    private readonly IInquiryHeaderRepository _headerRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IOrderDetailsRepository _orderDetails;
    private readonly IOrderHeaderRepository _orderHeader;
    private readonly IBrainTreeGate _brainTree;
    
    
    [BindProperty]
    public ProductUserVm ProductUserVm { get; set; }

    public CartController(
        IOrderDetailsRepository orderDetails,
        IOrderHeaderRepository orderHeader,
        IWebHostEnvironment webHostEnvironment,
        IInquiryDetailRepository detailRepository, 
        IApplicationUserRepository userRepo,
        IInquiryHeaderRepository headerRepository,
        IProductRepository productRepository,
        IBrainTreeGate brainTree
        )
    {
        _detailRepository = detailRepository;
        _headerRepository = headerRepository;
        _productRepository = productRepository;
        _userRepo = userRepo;
        _webHostEnvironment = webHostEnvironment;
        _orderDetails=orderDetails;
        _orderHeader=orderHeader;
        _brainTree = brainTree;
    }
    // GET
    public IActionResult Index()
    {
        List<ShoppingCart> shoppingCartsLIst = new List<ShoppingCart>();
        if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
            HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0
           )
        {
            shoppingCartsLIst=HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
        }

        List<int> prodInCart = shoppingCartsLIst.Select(i => i.ProductId).ToList();
        IEnumerable<Product> productsTempList = _productRepository.GetAll(u => prodInCart.Contains(u.Id));
        List<Product> prodList = new List<Product>();
        foreach (var cartObj in shoppingCartsLIst)
        {
            Product prodTemp = productsTempList.FirstOrDefault(u => u.Id == cartObj.ProductId);
            prodTemp.TempSqFt = cartObj.SqFt;
            prodList.Add(prodTemp);
        }
        return View(prodList);
    }

    public IActionResult Remove(int id)
    {
        List<ShoppingCart> shoppingCartsLIst = new List<ShoppingCart>();
        if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
            HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0
           )
        {
            shoppingCartsLIst=HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
        }
        shoppingCartsLIst.Remove(shoppingCartsLIst.FirstOrDefault(u=>u.ProductId == id));
        HttpContext.Session.Set(WC.SessionCart,shoppingCartsLIst);
        return RedirectToAction(nameof(Index));

    }
    
    [ValidateAntiForgeryToken] 
    [HttpPost,ActionName("Index")]
    public IActionResult IndexPost(IEnumerable<Product> prodList)
    {
        List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
        foreach (var product in prodList)
        {
            shoppingCartList.Add(new ShoppingCart{ ProductId = product.Id,SqFt = product.TempSqFt});
        }
        HttpContext.Session.Set(WC.SessionCart,shoppingCartList);
        return RedirectToAction(nameof(Summary));
    }

    public IActionResult Summary()
    {
        ApplicationUser applicationUser;
        if (User.IsInRole(WC.AdminRole))
        {
            if (HttpContext.Session.Get<int>(WC.SessionInquiryId) != 0)
            {
                InquiryHeader inquiryHeader = _headerRepository.FirstOrDefault(u =>
                    u.Id == HttpContext.Session.Get<int>(WC.SessionInquiryId));
                applicationUser = new ApplicationUser()
                {
                    Email = inquiryHeader.Email,
                    Fullname = inquiryHeader.FullName,
                    PhoneNumber = inquiryHeader.PhoneNumber
                };
            }
            else
            {
                applicationUser = new ApplicationUser();
            }

            var brain = _brainTree.GetGateWay();
            var clientToken = brain.ClientToken.Generate();
            ViewBag.ClientToken = clientToken;
        }
        else
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims =claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            applicationUser = _userRepo.FirstOrDefault(u => u.Id == claims.Value);
        }
        List<ShoppingCart> shoppingCartsLIst = new List<ShoppingCart>();
        if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
            HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
        {
            shoppingCartsLIst=HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
        }

        List<int> prodInCart = shoppingCartsLIst.Select(i => i.ProductId).ToList();
        List<Product> productsList = _productRepository.GetAll(u => prodInCart.Contains(u.Id)).ToList();
        ProductUserVm = new ProductUserVm()
        {
            ApplicationUser = applicationUser,
         };

        foreach (var cart in shoppingCartsLIst)
        {
            Product prodTemp = _productRepository.FirstOrDefault(u=> u.Id == cart.ProductId);
            prodTemp.TempSqFt = cart.SqFt;
            ProductUserVm.ProductList.Add(prodTemp);
        }
        return View(ProductUserVm);

    }
    
    [HttpPost,ActionName("Summary")]
    public IActionResult SummaryPost(IFormCollection collection,ProductUserVm productUserVm )
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (User.IsInRole(WC.AdminRole))
        {
            var orderTotal = 0.0;
            foreach (var product in ProductUserVm.ProductList)
            {
                orderTotal += product.Price * product.TempSqFt;
            }

            OrderHeader orderHeader = new OrderHeader()
            {
                CreatedByUserId = claim.Value,
                FinalOrderTotal = orderTotal,
                City = ProductUserVm.ApplicationUser.City,
                Email = ProductUserVm.ApplicationUser.Email,
                StreetAddress = ProductUserVm.ApplicationUser.StreetAddress,
                State = ProductUserVm.ApplicationUser.State,
                PostalCode = ProductUserVm.ApplicationUser.PostalCode,
                FullName = ProductUserVm.ApplicationUser.Fullname,
                PhoneNumber= ProductUserVm.ApplicationUser.PhoneNumber,
                OrderDate = DateTime.Now,
                OrderStatus = WC.StatusPending,
                TransactionId = "id_place_holder"
            };
            _orderHeader.Add(orderHeader);
            _orderHeader.Save();
            
            foreach (var prod in ProductUserVm.ProductList)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    OrderHeaderId = orderHeader.Id,
                    PricePerSqFt = prod.Price,
                    Sqft = prod.TempSqFt,
                    ProductId = prod.Id,
                };
                _orderDetails.Add(orderDetails);
            }
            _orderDetails.Save();
            string nonceFromClient = collection["payment_method_nonce"];
            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(orderHeader.FinalOrderTotal),
                PaymentMethodNonce = nonceFromClient,
                OrderId = orderHeader.Id.ToString(),
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
             };
             var gateway = _brainTree.GetGateWay();
             Result<Transaction> result = gateway.Transaction.Sale(request);
            
             if (result.Target.ProcessorResponseText == "Approved")
             {
                 orderHeader.TransactionId = result.Target.Id;
                 orderHeader.OrderStatus = WC.StatusApproved;
             }
             else
             {
                 orderHeader.OrderStatus = WC.StatusCancel;
             }
             _orderHeader.Save();
            return RedirectToAction(nameof(InquiryConfirmation), new {id=orderHeader.Id});
        }
        else
        {
            var PathToTemplates = _webHostEnvironment.WebRootPath
                                  + Path.DirectorySeparatorChar.ToString()
                                  + "templates" + Path.DirectorySeparatorChar.ToString() 
                                  + "Inquiry.html";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplates))
            {
                HtmlBody = sr.ReadToEnd();
            }

            StringBuilder productListSB = new StringBuilder();
            foreach (var prod in ProductUserVm.ProductList)
            {
                productListSB.Append($" - Name: {prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span> <br/> ");
            }

            string messageBody = string.Format(HtmlBody,
                ProductUserVm.ApplicationUser.Fullname,
                ProductUserVm.ApplicationUser.Email,
                ProductUserVm.ApplicationUser.PhoneNumber,
                productListSB.ToString()
            );
            //SEND EMAIL CODE (Message body is already defined ))
            //PARAMETER BEING LOGIN CREDENTIALS /SUBJECT /MESSAGE BODY 
            EmailSender.SendEmail(messageBody);

            InquiryHeader inquiryHeader = new InquiryHeader()
            {
                ApplicationUserId = claim.Value,
                FullName = ProductUserVm.ApplicationUser.Fullname,
                Email = ProductUserVm.ApplicationUser.Email,
                PhoneNumber = ProductUserVm.ApplicationUser.PhoneNumber,
                InquiryDate = DateTime.Now,
            };
            _headerRepository.Add(inquiryHeader);
            _headerRepository.Save();

            foreach (var prod in ProductUserVm.ProductList)
            {
                InquiryDetail inquiryDetail = new InquiryDetail()
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = prod.Id,
                };
                _detailRepository.Add(inquiryDetail);
                _detailRepository.Save();
            }
        }
        return RedirectToAction(nameof(InquiryConfirmation));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateCart(IEnumerable<Product> prodList)
    {
        List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
        foreach (var product in prodList)
        {
            shoppingCartList.Add(new ShoppingCart{ ProductId = product.Id,SqFt = product.TempSqFt});
        }
        HttpContext.Session.Set(WC.SessionCart,shoppingCartList);
        return RedirectToAction(nameof(Index));
    }
    public IActionResult InquiryConfirmation(int? id)
    {
        OrderHeader orderHeader = _orderHeader.FirstOrDefault(u => u.Id == id);
        HttpContext.Session.Clear();
        return View(orderHeader);
    }

    public IActionResult Clear()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

}