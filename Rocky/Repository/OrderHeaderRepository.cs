using Rocky.Data;
using Rocky.Models;
using Rocky.Repository.IRepository;

namespace Rocky.Repository;

public class OrderHeaderRepository:Repository<OrderHeader>,IOrderHeaderRepository
{
    private readonly RockyDbContext _db;
    public OrderHeaderRepository(RockyDbContext db) : base(db)
    {
        _db = db;
    }
    
    public void Update(OrderHeader obj)
    {
        _db.OrderHeader.Update(obj);
    }
}