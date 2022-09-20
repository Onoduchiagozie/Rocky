using Rocky.Data;
using Rocky.Models;
using Rocky.Repository.IRepository;

namespace Rocky.Repository;

public class CategoryRepository:Repository<Category>,ICategoryRepository
{
    private readonly RockyDbContext _db;
    public CategoryRepository(RockyDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Category obj)
    {
        var objFromDb = _db.Category.FirstOrDefault(u=>u.Id==obj.Id);
        if (objFromDb != null)
        {
            objFromDb.Name = obj.Name;
            objFromDb.DisplayOrder = obj.DisplayOrder;
        }
        
    }
}