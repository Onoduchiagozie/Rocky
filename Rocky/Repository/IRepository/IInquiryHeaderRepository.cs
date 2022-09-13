using Rocky.Models;

namespace Rocky.Repository.IRepository;

public interface IInquiryHeaderRepository:IRepository<InquiryHeader>
{
    void Update(InquiryHeader obj);


}