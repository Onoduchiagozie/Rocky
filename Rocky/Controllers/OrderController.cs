using Braintree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky_Utility;
using Rocky.Models;
using Rocky.Models.ViewModel;
using Rocky.Repository.IRepository;
using Rocky.Utility;

namespace Rocky.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderDetailsRepository _orderDetails;
        private readonly IOrderHeaderRepository _orderHeader;
        private readonly IBrainTreeGate _brainTree;
        
        [BindProperty]
        public OrderVm OrderVm { get; set; }

        public OrderController(IOrderDetailsRepository orderDetails, IOrderHeaderRepository orderHeader, IBrainTreeGate brainTree
        )
        {
            _orderDetails=orderDetails;
            _orderHeader=orderHeader;
            _brainTree = brainTree;
        }

        public IActionResult Index(string searchName=null, string SearchEmail=null, string SearchPhone=null,string Status= null)
        {
            OrderLIstVm orderLIstVm = new OrderLIstVm()
            {
                OrderHeaderList = _orderHeader.GetAll(),
                StatusLIst =WC.listStatus.ToList().Select(i=> new SelectListItem{Text = i,Value = i})
            };

            if (!string.IsNullOrEmpty(searchName))
            {
                orderLIstVm.OrderHeaderList = orderLIstVm.OrderHeaderList.Where(u => u.FullName.ToLower().Contains(searchName.ToLower()));
            }

            if (!string.IsNullOrEmpty(SearchEmail))
            {
                orderLIstVm.OrderHeaderList = orderLIstVm.OrderHeaderList.Where(u => u.Email.ToLower().Contains(searchName.ToLower()));
                
            }

            if (!string.IsNullOrEmpty(SearchPhone))
            {
                orderLIstVm.OrderHeaderList = orderLIstVm.OrderHeaderList.Where(u => u.PhoneNumber.ToLower().Contains(searchName.ToLower()));
                
            }

            if (!string.IsNullOrEmpty(Status) && Status != "--Order-Status")
            {
                orderLIstVm.OrderHeaderList = orderLIstVm.OrderHeaderList.Where(u => u.OrderStatus.ToLower().Contains(Status.ToLower()));
                
            }
            return View(orderLIstVm);
        }

        public IActionResult Details(int id)
        {
            OrderVm orderVm = new OrderVm()
            {
                OrderHeader = _orderHeader.FirstOrDefault(u => u.Id == id),
                OrderDetails = _orderDetails.GetAll(o => o.OrderHeaderId == id,includeProperties:"Product")
            };
            return View(orderVm);
        }
        
        [HttpPost]
        public IActionResult StartProcessing(OrderVm orderVm)
        {
            OrderHeader orderHeader = _orderHeader.FirstOrDefault(u => u.Id == orderVm.OrderHeader.Id);
            orderHeader.OrderStatus = WC.StatusInProgress;
            _orderHeader.Save();
            TempData[WC.Success] = "Order Processing Started";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        public IActionResult ShipOrder(OrderVm orderVm)
        {
            OrderHeader orderHeader = _orderHeader.FirstOrDefault(u => u.Id == orderVm.OrderHeader.Id);
            orderHeader.OrderStatus = WC.StatusShipped;
            orderHeader.ShippingDate=DateTime.Now;
            _orderHeader.Save();
            TempData[WC.Success] = "Order Shipped successfully";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        public IActionResult CancelOrder(OrderVm orderVm)
        {
            OrderHeader orderHeader = _orderHeader.FirstOrDefault(u => u.Id == orderVm.OrderHeader.Id);
            orderHeader.OrderStatus = WC.StatusCancel;
            var gateway = _brainTree.GetGateWay();
            Transaction transaction = gateway.Transaction.Find(orderHeader.TransactionId);

            if (transaction.Status == TransactionStatus.AUTHORIZED || transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
            {
                Result<Transaction> resultVoid = gateway.Transaction.Void(orderHeader.TransactionId);
            }
            else
            {
                Result<Transaction> resultRefund = gateway.Transaction.Void(orderHeader.TransactionId);
            }

            orderHeader.OrderStatus = WC.StatusRefunded;
            _orderHeader.Save();
            TempData[WC.Success] = "Order Cancelled Successfully";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        public IActionResult UpdateOrderDetails(OrderVm orderVm)
        {
            OrderHeader orderHeaderFromDb = _orderHeader.FirstOrDefault(u => u.Id == orderVm.OrderHeader.Id);
            orderHeaderFromDb.FullName = orderVm.OrderHeader.FullName;
            orderHeaderFromDb.PhoneNumber = orderVm.OrderHeader.PhoneNumber;
            orderHeaderFromDb.City = orderVm.OrderHeader.City;
            orderHeaderFromDb.StreetAddress = orderVm.OrderHeader.StreetAddress;
            orderHeaderFromDb.PostalCode = orderVm.OrderHeader.PostalCode;
            orderHeaderFromDb.State = orderVm.OrderHeader.State;
            orderHeaderFromDb.Email = orderVm.OrderHeader.Email;
            _orderHeader.Save();
            TempData[WC.Success] = "order details updated successfully";
            return RedirectToAction("Details","Order",new {id=orderHeaderFromDb.Id});
        }
    }
}