using Rocky.Data;
using Rocky.Models;
using Rocky.Repository.IRepository;

namespace Rocky.Repository;

public class ApplicationUserRepository:Repository<ApplicationUser>,IApplicationUserRepository
{
    private readonly RockyDbContext _db;
    public ApplicationUserRepository(RockyDbContext db) : base(db)
    {
        _db = db;
    }

}