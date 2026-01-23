using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Controllers
{
    // p.216 [Add] ナビゲーションプロパティへのアクセス
    public class EntityController : Controller
    {
        private readonly MyContext _db;

        public EntityController(MyContext db)
        {
            this._db = db;
        }

        // GET: EntityController
        public ActionResult Index()
        {
            return View();
        }

        // p.216 [Add] ナビゲーションプロパティへのアクセス（一括読み込み）
        public async Task<IActionResult> Assoc(int id=1)
        {
            //-----------------------------------
            // Books : Reviews : Authors : User
            //   1   :    n
            //   m        :         n
            //                      1    :   1
            //-----------------------------------
            // Includeメソッド    ：データベースへの問い合わせに際して、初期問い合せでまとめて関連データを取得。（一括読み込み）
            // ThenIncludeメソッド：ほぼ同上。その直前のIcludeメソッドで読み込まれた関連データを基点に、さらにその先の関連データを読み込む。
            var b = await _db.Books
                .Include(b => b.Reviews)       // Include    ：Books基点の参照
                .Include(b => b.Authors)       // Include    ：Books基点の参照
                .ThenInclude(a => a.User)      // ThenInclude：Authors基点の参照
                .SingleAsync(b => b.Id == id); // 

            return View(b);
        }

        // p.218 [Add] ナビゲーションプロパティへのアクセス（明示的読み込み）
        public async Task<IActionResult> Assoc2(int id=1)
        {
            //-----------------------------------
            // Books : Reviews : Authors : User
            //   1   :    n
            //   m        :         n
            //                      1    :   1
            //-----------------------------------
            // Entryメソッド     ：Includeメソッドに対して、任意のタイミングで関連データを読み込む。（明示的読み込み）
            // Collectionメソッド：コレクションナビゲーション。
            // LoadAsyncメソッド ：読み込んだ関連データをナビゲーションプロパティに流し込む。
            var b = await _db.Books.SingleAsync(b => b.Id == id);
            await _db.Entry(b).Collection(b => b.Reviews).LoadAsync();
            await _db.Entry(b).Collection(b => b.Authors).LoadAsync();

            return View(b);
        }

        // p.220 [Add] ナビゲーションプロパティへのアクセス（遅延読み込み）
        // ※あらかじめライブラリ（Microsoft.EntityFrameworkCore.Proxiesパッケージ）を
        //   インストールして有効にしておくことでInclude／Entryなどの準備が不要となる。
        public async Task<IActionResult> Assoc3(int id=1)
        {
            //-----------------------------------
            // Books : Reviews : Authors : User
            //   1   :    n
            //   m        :         n
            //                      1    :   1
            //-----------------------------------
            var b = await _db.Books.FindAsync(id); // ←関連データもまとめて取得

            return View(b);
        }

    }
}
