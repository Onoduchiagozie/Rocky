using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Rocky.Models;

public class ApplicationUser:IdentityUser
{
    public string Fullname { get; set; }
    [NotMapped]
    public string StreetAddress { get; set; }
    [NotMapped]
    public string City { get; set; }
    [NotMapped]
    public string State { get; set; }
    [NotMapped]
    public string PostalCode { get; set; }
}   