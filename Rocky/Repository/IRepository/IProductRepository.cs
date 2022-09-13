using Rocky.Models;

namespace Rocky.Repository.IRepository;

public interface IProductRepository:IRepository<Product>
{
    void Update(Product obj);
    IEnumerable<Category> getAllDropDownCategories();
    IEnumerable<ApplicationType> getAllDropDownApplicationTypes();


}