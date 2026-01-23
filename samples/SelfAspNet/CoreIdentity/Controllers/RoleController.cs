using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentity.Controllers;

public class RoleController : Controller
{
    private RoleManager<IdentityRole> _role;
    private UserManager<IdentityUser> _usr;

    public RoleController(
      RoleManager<IdentityRole> role, UserManager<IdentityUser> usr)
    {
        _role = role;
        _usr = usr;
    }

    [Authorize]
    public async Task<IActionResult> Create()
    {
        var roleName = "Admin";
        var exist = await _role.RoleExistsAsync(roleName);
        if (!exist)
        {
            await _role.CreateAsync(new IdentityRole("Admin"));
        }

        var current = await _usr.GetUserAsync(User);
        if (current != null)
        {
            await _usr.AddToRoleAsync(current, roleName);
        }
        return Content("現在のユーザーをAdminロールに登録しました。");
    }
}
