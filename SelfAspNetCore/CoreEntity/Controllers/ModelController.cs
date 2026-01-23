using CoreEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreEntity.Controllers
{
    public class ModelController : Controller
    {
        private readonly MyContext _db;

        public ModelController(MyContext db)
        {
            this._db = db;
        }

        // p.232 継承関係にあるエンティティのマッピング
        public IActionResult Inherit()
        {
            // 記事エンティティから追加
            _db.Articles.Add(new 
                Article{
                    Url = "https://wings.msn.to/Java/",
                    Title = "Java入門"
                }
            );

            // タイアップ記事エンティティから追加
            _db.CollabArticles.Add(new 
                CollabArticle{
                    Url = "https://wings.msn.to/ad/",
                    Title = "ASP.NET Core入門",
                    Company = "WINGS",
                }
            );

            // データベースに反映
            _db.SaveChanges();

            return Content("データを保存しました。");
        }

        // public async Task<IActionResult> LocalEmail()
        // {
        //     var us = await _db.Users
        //       .SingleAsync(u => u.Id == 1);
        //     us.Email = new EmailAddress("yoshihiro@example.com");
        //     await _db.SaveChangesAsync();
        //     return Content(us.Email!.Local);
        // }

        // public IActionResult ViewPub()
        // {
        //     var vp = _db.ViewPubCounts;
        //     return View(vp);
        // }




        // public async Task<IActionResult> ViewPub()
        // {
        //     var vp = await _db.ViewPubCounts.ToListAsync();
        //     return View(vp);
        // }

    }
}
