using Microsoft.AspNetCore.Mvc;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Controllers
{
    public class CommonController : Controller
    {
        private readonly MyContext _db;

        public CommonController(MyContext db)
        {
            this._db = db;
        }

        // GET: CommonController
        public ActionResult Index()
        {
            return View();
        }

        // GET: common/nest
        public ActionResult Nest()
        {
            return View();
        }

        // GET: common/list
        // p.203 [Add] テンプレート化されたRazorデリゲート
        public ActionResult List()
        {
            // 書籍情報を取得
            return View(_db.Books);
        }

        // GET: common/list2
        // p.205 [Add] Razorデリゲートをビューヘルパーに渡す
        public ActionResult List2()
        {
            // 書籍情報を取得
            return View(_db.Books);
        }

    }
}
