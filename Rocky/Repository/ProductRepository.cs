using Rocky.Data;
using Rocky.Models;
using Rocky.Repository.IRepository;

namespace Rocky.Repository;

public class ProductRepository:Repository<Product>,IProductRepository
{
    private readonly RockyDbContext _db;
    public ProductRepository(RockyDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Product obj)
    {
        var objFromDb = _db.Category.FirstOrDefault(u=>u.Id==obj.Id);
        if (objFromDb != null)
        {
            _db.Products.Update(obj);
        }
    }

    public IEnumerable<Category> getAllDropDownCategories()
    {
        return _db.Category.ToList();
    }

    public IEnumerable<ApplicationType> getAllDropDownApplicationTypes()
    {
        return _db.ApplicationTypes.ToList();
    }
    
}