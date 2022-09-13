namespace Rocky.Models.ViewModel;

public class HomeVM
{
    public IEnumerable<Product> Product { get; set; }
    public IEnumerable<Category> Categorie { get; set; }
}