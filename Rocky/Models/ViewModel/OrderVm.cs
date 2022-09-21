namespace Rocky.Models.ViewModel;

public class OrderVm
{
    public OrderHeader OrderHeader { get; set; }
    public IEnumerable<OrderDetails> OrderDetails { get; set; }
}