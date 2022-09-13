using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocky.Models.ViewModel;

public class ProductVM
{
    public IEnumerable<Category>? CategorySelectList { get; set; }
    public IEnumerable<ApplicationType>? ApplicationTypes { get; set; }
    public Product Product { get; set; }
}