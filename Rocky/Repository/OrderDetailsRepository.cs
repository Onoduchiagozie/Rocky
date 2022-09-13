using Rocky.Data;
using Rocky.Models;
using Rocky.Repository.IRepository;

namespace Rocky.Repository;

public class OrderDetailRepository:Repository<OrderDetails>,IOrderDetailsRepository
{
    private readonly RockyDbContext _db;
    public OrderDetailRepository(RockyDbContext db) : base(db)
    {
        _db = db;
    }
    
    public void Update(OrderDetails obj)
    {
        _db.OrderDetails.Update(obj);
    }
}