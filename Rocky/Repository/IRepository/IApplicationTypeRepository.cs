using Rocky.Models;

namespace Rocky.Repository.IRepository;

public interface IApplicationTypeRepository:IRepository<ApplicationType>
{
    void Update(ApplicationType obj);
}