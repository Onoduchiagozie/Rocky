using Rocky.Data;
using Rocky.Models;
using Rocky.Repository.IRepository;

namespace Rocky.Repository;

public class ApplicationRepository:Repository<ApplicationType>,IApplicationTypeRepository
{
    private readonly RockyDbContext _db;
    public ApplicationRepository(RockyDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(ApplicationType obj)
    {
        var objFromDb = _db.ApplicationTypes.FirstOrDefault(u=>u.Id==obj.Id);
        if (objFromDb != null)
        {
            objFromDb.Name = obj.Name;
        }
    }
}