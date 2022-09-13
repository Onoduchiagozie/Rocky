using Rocky.Models;

namespace Rocky.Repository.IRepository;

public interface IOrderHeaderRepository:IRepository<OrderHeader>
{
    void Update(OrderHeader obj);


}