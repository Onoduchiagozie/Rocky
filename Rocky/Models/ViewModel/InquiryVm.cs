namespace Rocky.Models.ViewModel;

public class InquiryVm
{
    public InquiryHeader InquiryHeader { get; set; }
    public IEnumerable<InquiryDetail> InquiryDetails { get; set; }
}