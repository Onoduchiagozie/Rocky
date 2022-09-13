using Rocky.Data;
using Rocky.Models;
using Rocky.Repository.IRepository;

namespace Rocky.Repository;

public class InquiryHeaderRepository:Repository<InquiryHeader>,IInquiryHeaderRepository
{
    private readonly RockyDbContext _db;
    public InquiryHeaderRepository(RockyDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(InquiryHeader obj)
    {
        _db.InquiryHeader.Update(obj);
    }
    
    
}