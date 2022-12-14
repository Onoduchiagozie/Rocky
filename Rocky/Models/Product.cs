using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocky.Models;

public class Product
{
    public Product()
    {
        TempSqFt = 50; 
    }
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string ShortDesc { get; set; }
    public string Description { get; set; }
    [Range(1,int.MaxValue)]
    public double Price { get; set; }
    public string Image { get; set; }
    [Display(Name = "Category Type")]
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public  Category? Category { get; set; }
    
    [Display(Name = "Application Type")]
    public int ApplicationTypeId { get; set; }
    [ForeignKey("ApplicationTypeId")]
    public  ApplicationType? ApplicationType  { get; set; }
    [NotMapped]
    [Range(50,1000)]
    [Display(Name = "KG")]
    [Required(ErrorMessage = "Any Order Less Than 50KG Is Not Allowed")]
    public int TempSqFt { get; set; }
}