using Rocky.Data;
using Rocky.Models;
using Rocky.Repository.IRepository;

namespace Rocky.Repository;

public class InquiryDetailRepository:Repository<InquiryDetail>,IInquiryDetailRepository
{
    private readonly RockyDbContext _db;
    public InquiryDetailRepository(RockyDbContext db) : base(db)
    {
        _db = db;
    }
    
    public void Update(InquiryDetail obj)
    {
        _db.InquiryDetail.Update(obj);
    }
}