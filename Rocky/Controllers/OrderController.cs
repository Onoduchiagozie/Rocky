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
    }
}