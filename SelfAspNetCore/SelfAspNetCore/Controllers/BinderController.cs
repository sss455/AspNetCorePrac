using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SelfAspNetCore.Lib.MyModelBinder;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Controllers;

// p.369 [Add] モデルバインド

 public class BinderController : Controller
 {
    private readonly MyContext _db;

    // p.378 [Add] ファイルをアップロードする(1) ーーファイルシステムへの保存
    private readonly IWebHostEnvironment _host;

    // コンストラクター
    public BinderController(MyContext db, IWebHostEnvironment host)
    {
        _db = db;
        _host = host;
    }

     // GET: BinderController
     public ActionResult Index()
     {
         return View();
     }

    // 初期表示
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) { return NotFound(); }

        var book = await _db.Books.FindAsync(id);
        if (book == null) { return NotFound(); }

        return View(book);
    }

    // p.372 [Add] Bind属性 (2)更新時の注意点ーーTryUpdateModelAsyncメソッド
    // 編集画面：[Save]ボタンで編集内容をデータベースに反映
    [HttpPost]
    [ValidateAntiForgeryToken]
    // ポストデータを引数にバインド
    public async Task<IActionResult> Edit(int id) //,
                                        // オーバーポスティング攻撃対策のため、引数Bookエンティティ(モデルオブジェクト)にバインドしてよいプロパティを明示的に列挙
                                        // [Bind("Id,Isbn,Title,Price,Publisher,Published,Sample,RowVewsion")] Book book)
    {
        // p.372 [Add] Bind属性 (2)更新時の注意点ーーTryUpdateModelAsyncメソッド
        // id値で既存の書籍を取得
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
        if(book == null)
        {
            return NotFound();
        }

        // Isbn～Sampleプロパティを対応するソース値で更新
        // 引数へのバインド(アクション呼び出しのタイミングで実行)を以下のように書き換えることもできる。
        // ※TryUpdateModelAsync：モデルバインドを任意のタイミングで実行する。更新が成功したかどうかをブール値で返す。
        //                        更新(モデルバインド)が成功した場合に、データベースに更新を反映させる。
        if(await TryUpdateModelAsync(
                    model : book,   // 更新対象のモデル
                    prefix: "",     // 値プロバイダーで利用するプレフィックス
                 // バインドするプロパティ(可変長引数)
                 // params Expression<Func<TModel, object?>>[] includeExpressions:
                            b => b.Isbn,
                            b => b.Title,
                            b => b.Price,
                            b => b.Publisher,
                            b => b.Published,
                            b => b.Sample))
        {

            // 検証に成功したらデータベース処理＆リダイレクト
            if (ModelState.IsValid)
            {
                try
                {
                    // エンティティの更新ををデータベースに反映（メモリ上の更新操作）
                    _db.Update(book);

                    // SaveChangesAsyncメソッドでデータベースへ反映（実操作）
                    await _db.SaveChangesAsync();
                }
                // 競合が発生した場合の処理
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                // データベース更新に成功した場合はリダイレクト
                return RedirectToAction(nameof(Index));
            }
            
        }

        // 入力に問題がある場合は編集フォームを再描画
        return View(book);
    }



    // p.272 [Add] リストへのバインド
    // 初期表示
    public IActionResult CreateMulti()
    {
        return View();
    }

    // [登録]ボタンクリック時に呼び出され、登録処理を実行
    // ※複数値を受け取るために、IEnumerable<Book>型の引数を用意。
    [HttpPost]
    public async Task<IActionResult> CreateMulti(IEnumerable<Book> list)
    {
        for(var i=0; i < list.Count(); i++)
        {
            // リストから順にモデルを取り出す
            Book b = list.ElementAt(i);

            // ISBNが未入力の場合は、未入力行と見なしスキップ
            if(string.IsNullOrEmpty(b.Isbn))
            {
                // 未処理の行は検証情報も不要のため、ModelStateからエラー情報を除去
                foreach(var key in new [] {"Isbn", "Title", "Price", "Publisher", "Published", "Sample"})
                {
                    // Removeメソッドのキーもフォームに対応した「list[0].Isbn」のような形式
                    ModelState.Remove($"list[{i}].{key}");
                }
                continue;
            }
            // 入力された行のみをDBコンテキストに追加
            _db.Books.Add(b);
        }

        // エラーがなければデータベースに反映
        if(ModelState.IsValid)
        {
            // DBコンテキストの内容をデータベースに反映
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(CreateMulti));
        }
        else
        {
            return View(); // 検証失敗時にはフォームを再描画
        }
    }


    // p.272 [Add] IDictionary型へのバインド
    // 初期表示
    public IActionResult CreateMulti2()
    {
        return View();
    }

    // [登録]ボタンクリック時に呼び出され、登録処理を実行
    // ※複数値を受け取るために、IEnumerable<Book>型の引数を用意。
    [HttpPost]
    public async Task<IActionResult> CreateMulti2(IDictionary<string, Book> dic)
    {
        foreach(var kv in dic)
        {

            // ISBNが未入力の場合は、未入力行と見なしスキップ
            if(string.IsNullOrEmpty(kv.Value.Isbn))
            {
                ModelState.Remove($"dic[{kv.Key}].Key");

                // 未処理の行は検証情報も不要のため、ModelStateからエラー情報を除去
                foreach(var key in new [] {"Isbn", "Title", "Price", "Publisher", "Published", "Sample"})
                {
                    // Removeメソッドのキーもフォームに対応した「dic[0].Value.Isbn」のような形式
                    ModelState.Remove($"dic[{kv.Key}].Value.{key}");
                }
                continue;
            }
            // 入力された行のみをDBコンテキストに追加
            _db.Books.Add(kv.Value);
        }

        // エラーがなければデータベースに反映
        if(ModelState.IsValid)
        {
            // DBコンテキストの内容をデータベースに反映
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(CreateMulti2));
        }
        else
        {
            return View(); // 検証失敗時にはフォームを再描画
        }
    }


    // p.378 [Add] ファイルをアップロードする(1) ーーファイルシステムへの保存
    // 初期表示
    public IActionResult Upload()
    {
        return View();
    }

    // アップロード処理
    // ※アップロード時は、型だけがアップロードファイル向けの型となる（IFormFile型）
    //  ・単数ファイルの場合：IFormFile型
    //  ・複数ファイルの場合：List<IFormFile>型
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(List<IFormFile>? upFiles)
    {
        // ファイルが指定されていない場合はエラー
        if(upFiles == null || upFiles.Count() == 0)
        {
            ModelState.AddModelError(string.Empty, "ファイルが指定されていません。");
            return View();
        }

        // アップロードに成功したファイル数
        int success = 0;

        // アップロードされたファイル群をforeachループで走査
        foreach(IFormFile file in upFiles)
        {
            var fileName = file.FileName;

            // アップロードファイルの種類(拡張子)をチェック
            var ext = new[] {".jpg", ".jpeg", ".png"};
            if( !ext.Contains(Path.GetExtension(fileName)) )
            {
                ModelState.AddModelError(string.Empty, $"拡張子は .png、.jpg でなければいけません（{fileName}）");
                continue;
            }

            // アップロードファイルのサイズをチェック
            if(file.Length > 1024 * 1024)
            {
                ModelState.AddModelError(string.Empty, $"ファイルサイズは1MB以内でなければいけません（{fileName}）");
                continue;
            }

            // アップロードファイルの保存先
            // ※「＜プロジェクトルート＞\Data\＜ファイル名＞」のようなパスを生成する。
            //    プロジェクトルートは、IWebHostEnvironment#ContentRootPathで取得できるので、
            //    IWebHostEnvironmentオブジェクトを注入しておく。
            string path = @$"{_host.ContentRootPath}\Data\{fileName}";

            // FileStream経由でアップロードファイルを複製
            // ※保存先のパスを元にFileStreamオブジェクトを生成し、CopyToAsyncメソッドでデータを流し込む。IFormFile⇒FileStreamへコピー
            //   ファイルの内容をFileStreamに複製。IFormFile⇒FileStreamへコピー
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            success++;
        }

        // 成功メッセージ＆フォームを再描画
        ViewBag.Message = $"{success}個のファイルをアップロードしました。";
        return View();
    }


    // p.382 [Add] ファイルをアップロードする(2) ーーデータベースへの保存
    // 初期表示
    public IActionResult UploadDB()
    {
        return View();
    }

    // アップロード処理（データベースへの保存）
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadDB(List<IFormFile>? upFiles)
    {
        if(upFiles == null || upFiles.Count() == 0)
        {
            ModelState.AddModelError(string.Empty, "ファイルが指定されていません。");
            return View();
        }

        int success = 0;

        // p.383 [Add] ファイルをアップロードする(2) ーーデータベースへの保存
        var photos = _db.Photos;

        // アップロードされたファイル群をforeachループで走査
        foreach(IFormFile file in upFiles)
        {
            var fileName = file.FileName;

            var ext = new[] {".jpg", ".jpeg", ".png"};
            if( !ext.Contains(Path.GetExtension(fileName)) )
            {
                ModelState.AddModelError(string.Empty, $"拡張子は .png、.jpg でなければいけません（{fileName}）");
                continue;
            }

            if(file.Length > 1024 * 1024)
            {
                ModelState.AddModelError(string.Empty, $"ファイルサイズは1MB以内でなければいけません（{fileName}）");
                continue;
            }

            // p.383 [Add] ファイルをアップロードする(2) ーーデータベースへの保存
            // アップロードファイルを、一時的なメモリにコピー
            // ※ファイルの内容をMemoryStreamに複製。IFormFile⇒MemoryStreamへコピー
            using var memory = new MemoryStream();
            await file.CopyToAsync(memory);
 
            // DBコンテキストにPhotoエンティティを追加
            photos.Add(
                new Photo{
                    Name        = Path.GetFileName(file.FileName), // ファイル名
                    ContentType = file.ContentType,                // コンテンツタイプ
                    Content     = memory.ToArray(),                // ファイル本体（ファイルの内容をコピーしたMemoryStreamをバイト配列に変換して設定）
                }
            );
            
            success++;
        }

        // アップロードファイルをまとめて保存（データベースに反映）
        await _db.SaveChangesAsync();

        // 成功メッセージ＆フォームを再描画
        ViewBag.Message = $"{success}個のファイルをアップロードしました。";
        return View();
    }


    // p.394 [Add] モデルバインダーの自作
    // 初期表示
    public IActionResult Custom()
    {
        return View();
    }

    // 入力値を受け取り、その結果を表示
    [HttpPost]
    public IActionResult Custom( 
                             // 特定の引数に対してバインダーを有効にするには、ModelBinder属性で修飾する。
                             // これで引数currentはDateModelBinderによってバインドされるようになる。
                             [ModelBinder(typeof(DateModelBinder))] DateTime current )
    {
        return Content($"入力値：{current.ToShortDateString()}");
    }


    // p.396 [Add] 自作のモデルバインダーをアプリ全体に適用する
    // 初期表示
    public IActionResult Custom2()
    {
        return View();
    }

    // 入力値を受け取り、その結果を表示
    [HttpPost]
    public IActionResult Custom2(
                            /*[ModelBinder(typeof(DateModelBinder))]*/ DateTime current )
    {
        return Content($"入力値：{current.ToShortDateString()}");
    }
}