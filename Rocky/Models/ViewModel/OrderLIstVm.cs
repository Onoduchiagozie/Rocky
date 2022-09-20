using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocky.Models.ViewModel;

public class OrderLIstVm
{
    public IEnumerable<OrderHeader> OrderHeaderList { get; set; }
    public IEnumerable<SelectListItem> StatusLIst { get; set; }
    public string Status { get; set; }
}