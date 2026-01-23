// p.422 [Add] アクションに認証近状を付与するーーAuthorize属性
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentity.Controllers;

public class RoleController : Controller
{
    //「RoleManager(ロールを操作)」「UserManager(ユーザーを操作) 」を準備
    private readonly RoleManager<IdentityRole> _role;
    private readonly UserManager<IdentityUser> _usr;

    public RoleController( RoleManager<IdentityRole> role,
                           UserManager<IdentityUser> usr  )
    {
        // それぞれサービスとして登録されているので、依存性を注入
        _role = role;
        _usr  = usr;
    }

    public async Task<IActionResult> Create()
    {
        // ロール名
        var roleName = "Admin";

        // 指定のロール(Admin)が存在するか
        bool adminRoleExist = await _role.RoleExistsAsync(roleName);
        // 存在しない場合
        if( !adminRoleExist )
        {
            // "Admin"ロールを新規作成
            await _role.CreateAsync( new IdentityRole(roleName) );
        }

        // ログインユーザーを取得
        // ※Controller＃Userプロパティで、ログインユーザーをClaimsPrincipal型で取得できる
        IdentityUser? loginUsr = await _usr.GetUserAsync(User);
        if(loginUsr != null)
        {
            // ログインユーザーをAdminロールに追加
            await _usr.AddToRoleAsync(loginUsr, roleName);
        }

        return Content("現在のユーザーをAdminロールに登録しました。");
    }
}
