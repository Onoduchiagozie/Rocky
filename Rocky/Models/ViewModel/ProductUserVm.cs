namespace Rocky.Models.ViewModel;

public class ProductUserVm
{
    public ProductUserVm()
    {
        ProductList = new List<Product>();
    }
    public ApplicationUser ApplicationUser { get; set; }
    public List<Product> ProductList { get; set; }
}