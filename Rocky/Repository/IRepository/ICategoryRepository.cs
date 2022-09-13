using Rocky.Models;

namespace Rocky.Repository.IRepository;

public interface ICategoryRepository:IRepository<Category>
{
    void Update(Category obj);
}