using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rocky_Utility;
using Rocky.Models;

namespace Rocky.Data.Initialiser;

public class DbInitializer:IDbInitializer
{
    private readonly RockyDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public DbInitializer(RockyDbContext db,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
        try
        {
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
        }

        if (!_roleManager.RoleExistsAsync(WC.AdminRole).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(WC.AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(WC.AdminRole)).GetAwaiter().GetResult();
        }
        else
        {
            return;
        }

        _userManager.CreateAsync(new ApplicationUser
        {
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            Fullname = "qwertyAdmin",
            PhoneNumber = "123456789",
        }, "Qwerty1>").GetAwaiter().GetResult();
        ApplicationUser user = _db.ApplicationUser.FirstOrDefault(u=>u.Email=="admin@gmail.com");
        _userManager.AddToRoleAsync(user, WC.AdminRole).GetAwaiter().GetResult();
    }
}