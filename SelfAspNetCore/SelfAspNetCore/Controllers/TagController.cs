using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using SelfAspNetCore.Helpers;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Controllers
{
    public class TagController : Controller
    {
        private readonly MyContext _db;

        // p.181 [Add] タグヘルパーコンポーネント
        private readonly ITagHelperComponentManager _manager;

        // コンストラクター
        public TagController(MyContext context, ITagHelperComponentManager manager)
        {
            // コンテキストの注入
            this._db = context;
            // p.181 [Add] タグヘルパーコンポーネントのサービスを注入
            this._manager = manager;
        }

        public IActionResult Index()
        {
            // p.181 [Add] 自作のタグヘルパーコンポーネントを登録
            _manager.Components.Add(new MetaTagHelperComponent(_db));

            return View();
        }

        public async Task<IActionResult> SelectGroup()
        {
            // 記事情報の取得
            var articles = _db.Articles.Select(a => new
            {
               Url      = a.Url,      // 記事URL
               Title    = a.Title,    // 記事タイトル
               Category = a.Category, // カテゴリ
            });

            // 選択オプションを作成（グループ化）
            //ViewBag.Opts = new SelectList(articles, "Url", "Title", null, "Category");
            ViewBag.Opts = new SelectList(
                                    items         : articles,    // リストの各SelectListItemを構築するために使用される項目
                                    dataValueField: "Url",       // <option>要素のvalue属性の設定値
                                    dataTextField : "Title",     // <option>要素のテキストフィールドの設定値
                                    selectedValue : null,        // <option>要素の初期選択値
                                    dataGroupField: "Category"); // <optgroup>要素の設定値（選択肢をグループ化するフィールド）

            return View(await _db.Articles.FindAsync(1));
        }

        // p.xxx [Add] Anchorタグヘルパー
        public IActionResult Anchor()
        {
            return View();
        }

        // p.xxx [Add] 
        public IActionResult Path()
        {
            return View();
        }

        // p.151 [Add] Cacheタグヘルパー(vary-by属性)
        public IActionResult Cache(int num1, int num2)
        {
            ViewBag.Num1 = num1;
            ViewBag.Num2 = num2;
            return View();
        }

        // p.153 [Add] 自作のビューヘルパー（Truncateメソッド）
        // p.158 [Add] 自作のビューヘルパー（Coverメソッド）
        public IActionResult CustomViewHelper()
        {
            return View();
        }

        // p.159 [Add] 自作のビューヘルパー（RadioButtonListForメソッド）
        public async Task<IActionResult> MyRadio(int? id)
        {
            // 重複なしの出版社名を取得
            ViewBag.Pubs = _db.Books
              .Select( b => new SelectListItem { 
                                    Value = b.Publisher,  // value属性
                                    Text  = b.Publisher   // 表示テキスト
                                } )
              .Distinct();

            // 引数idに合致する書籍を取得
            return View( await _db.Books.FindAsync(id) );
        }

        // p.164 [Add] 自作のビューヘルパー（@functionディレクティブ）
        public IActionResult Func()
        {
            return View();
        }

        // p.169 [Add] 自作のタグヘルパー（Coverヘルパー）
        public IActionResult Cover()
        {
            ViewBag.Isbn = "978-4-7981-7556-0";
            return View();
        }
    }
}
