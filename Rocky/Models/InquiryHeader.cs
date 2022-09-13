using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocky.Models;

public class InquiryHeader
{
    [Key]
    public int Id { get; set; }
    [Required]
    public String ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    public ApplicationUser ApplicationUser { get; set; }
    public DateTime InquiryDate { get; set; }
    public String PhoneNumber { get; set; }
    public String FullName { get; set; }
    public String Email { get; set; }
    
}