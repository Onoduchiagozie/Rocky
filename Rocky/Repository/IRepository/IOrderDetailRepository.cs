using Rocky.Models;

namespace Rocky.Repository.IRepository;

public interface IOrderDetailsRepository:IRepository<OrderDetails>
{
    void Update(OrderDetails obj);
}