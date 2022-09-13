using Rocky.Models;

namespace Rocky.Repository.IRepository;

public interface IInquiryDetailRepository:IRepository<InquiryDetail>
{
    void Update(InquiryDetail obj);
}