using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SelfAspNetCore.Lib.MyActionResult;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Controllers;

// p.336 [Add] 6.1 IActionResultオブジェクト
public class ResultController : Controller
{
    private readonly MyContext _db;

    // p.355 [Add] プラウザーキャッシュを活用するーー応答ヘッダーEtag、Last-Modified
    // ※IWebHostEnvironment：アプリルート(wwwrootフォルダー)を管理しているサービス(ASP.NET Core標準)
    private readonly IWebHostEnvironment _host;

    // コンストラクター
    public ResultController(MyContext db, IWebHostEnvironment host)
    {
        // コンテキストの注入
        _db = db;

        // p.355 [Add] プラウザーキャッシュを活用するーー応答ヘッダーEtag、Last-Modified
        // 依存性の注入
        _host = host;
    }

    // GET: ResultController
    public ActionResult Index()
    {
        return View();
    }

    // p.337 [Add] ページを部分更新するーーPartialViewResultクラス
    // 初期表示時にフォームを生成
    public ActionResult AjaxForm()
    {
        return View();
    }

    // p.337 [Add] ページを部分更新するーーPartialViewResultクラス
    // [検索]ボタンクリック時に、検索＆結果を部分ビューとして応答
    [HttpPost]
    public IActionResult AjaxSearch(string keyword, bool? released)
    {
        // Booksを全件検索
        var bs = _db.Books.Select(b =>b);

        // [キーワード]欄が空でない場合、その内容で部分一致検索
        if( !string.IsNullOrEmpty(keyword) )
        {
            bs = bs.Where(b => b.Title.Contains(keyword));
        }

        // [刊行済み]がチェック状態の場合、今日以前の刊行日の書籍で絞り込み
        if(released.HasValue && released.Value)
        {
            bs = bs.Where(b => b.Published <= DateTime.Now);
        }

        // 検索結果を部分ビューとして応答
        return PartialView("_AjaxResult", bs);
    }


    // p.339 [Add] ページをリダイレクトするーーRedirectResultクラス
    // Redirectヘルパー
    public IActionResult Move()
    {
        // リダイレクト先は、必ずしもアプリの配下でなくてもよい
        return Redirect("https://wings.msn.to/");
    }


    // p.341 [Add] オープンリダイレクト脆弱性を防ぐーーLocalRedirectヘルパー
    public IActionResult Local()
    {
        // リモートアドレス(外部サイト)へのリダイレクトはエラー
        return LocalRedirect("https://example.com/redirect?path=https://wings.msn.to/");
        // ※主にオープンリダイレクト脆弱性を防ぐため。
        //   外部からの入力によって自由なページ(サイト)に移動できてしまう静寂性のこと。
    }


    // p.342 [Add] リダイレクト先をアクション名で指定するーーRedirectToActionResultクラス
    // RedirectToActionヘルパー
    // p.343 [Add] ルート名／ルートパラメーターでリダイレクト先を決定するーーRedirectToRouteヘルパー
    // ※「Controllers/BooksController.cs」でリダイレクトする場合
    public IActionResult MoveAction()
    {
        //---------------------------------------------------------------------------------------------------
        //【構文】RedirectToActionメソッド
        //  (1) public RedirectToActionResult RedirectToAction([string? actionName [, object? routeValues]])
        //  (2) public virtual                RedirectToAction( string? actionName [, string? ControllerName 
        //                                                                            [, object? routeValues] [, string? fragment]
        //                                                                         ])
        //   ・actionName    ：アクション名
        //   ・routeValues   ：ルートパラメーター（匿名型「new { 名前=値 }」）
        //   ・controllerName：コントローラー名
        //   ・fragment      ：#フラグメント
        //---------------------------------------------------------------------------------------------------
        // 異なるコントローラーに移動する
        /*return*/ RedirectToAction("Index", "Hello");
        // ・第1引数「アクション名」
        // ・第2引数「異なるコントローラー名(Hello)」

        // パラメーターを付与する場合「～/Books/Details/108?charset=utf-8」
        /*return*/ RedirectToAction(nameof(Details), new { id=108, charset="utf-8" });
        // ・第1引数「アクション名」
        // ・第2引数「ルートパラメーター(匿名型)」

        // #フラグメントを付与する場合「～/Book#supplement」
        /*return*/ RedirectToAction(nameof(Index), null, "supplement");
        // ・第1引数「アクション名」
        // ・第2引数「コントローラー名」は省略できないのでnull(明示的に名前を指定しても可)
        // ・第3引数「#フラグメント」

        //---------------------------------------------------------------------------------------------------
        //【構文】RedirectToRouteメソッド
        //  public virtual RedirectToAction([string? routeName [, object? routeValues] [, string? fragment]])
        //  ・routeName     ：ルート名
        //  ・routeValues   ：ルートパラメーター（匿名型「new { 名前=値 }」）
        //  ・fragment      ：#フラグメント
        //---------------------------------------------------------------------------------------------------
        // リダイレクト先を「ルート名＋ルートパラメーター(匿名型)」の組み合わせで指定する場合（「～/Books/Details/108?charset=utf-8」）
        /*return*/ RedirectToRoute("default", new { controller="Books", action="Details", id=108, charset="utf-8" });
        // ・第1引数「ルート名」                 ※defaultはプロジェクト既定で用意されているルート定義の名前（Program.cs）
        // ・第2引数「ルートパラメーター(匿名型)」 ※コントローラー／アクションまで、すべてをルートパラメーターとして指定
        // ※ルート名によってコントローラー／アクションが決まる(＝追加のルートパラメーターだけを渡せばよい)場合に威力を発揮する
        //  （独自のルート定義については8.1節）

        return Empty;
    }
    public IActionResult Details() {return Empty;}


    // p.344 [Add] ステータスコードだけを出力するーーHttpStatusCodeResultクラス
    // StatusCodeヘルパー
    public async Task<IActionResult> Status(int? id)
    {
        var bs = await _db.Books.FindAsync(id);

        // 該当データが存在しない場合
        if(bs == null)
        {   
            // ステータスコード「404 Not Found」エラーを返す
            return StatusCode(404);

            // StatusCodesクラスの定数を使用すると、設定値の意図はより明確になる
            //return StatusCode(StatusCodes.Status404NotFound);

            // ※ただし、StatusCodeヘルパーを直接に利用する機会はそこまで多くはない。
            //   よく利用するステータスコードに特化した専用ヘルパーが用意されているため。
            //  （詳細は、p346の表6.4を参照）
        }

        return View("../Books/Details", bs);
    }


    // p.346 [Add] アクションをそのまま終了するーーEmptyResultクラス
    // Emptyヘルパープロパティ
    public IActionResult Nothing()
    {
        //「～/result/nothing」にアクセスすると、プラウザーには空のページが表示される
        // ⇒アクションの結果を空(Empty)として返している
        return Empty;

        // ※コンテンツを返さないことを明確にする場合、「NoContentヘルパー」を利用するのが望ましいため、ほぼ利用する機会はない。
        //  （数少ない用途については6.1.8項）
    }


    // p.347 [Add] テキストのコンテンツを出力するーーContentResultクラス
    // Emptyヘルパープロパティ
    public IActionResult Plain()
    {
        //---------------------------------------------------------------------------------------------------
        //【構文】Contentメソッド
        //  public ContentResult Content(string content, string contentType, Encoding contentEncoding)
        //  ・content        ：出力する文字列
        //  ・contentType    ：コンテンツタイプ（既定はtext/plain）
        //  ・contentEncoding：文字エンコーディング
        //---------------------------------------------------------------------------------------------------
        /*return*/ Content("こんにちは、世界！", "text/plain", Encoding.UTF8);

        // 引数contentTypeは「MediaTypeNames.Text／MediaTypeNames.Applicationクラス」を利用して、以下のように表しても構わない
        return Content("こんにちは、世界！", System.Net.Mime.MediaTypeNames.Text.Plain, Encoding.UTF8);
        // ※コードは若干ながくなるが、インテリセンスの恩恵を得られるので、タイプミスを防ぎやすくなる
        //  「MediaTypeNames.Text／MediaTypeNames.Applicationクラス」の主なフィールドは p.348の表6.5を参照。
    }
    // より簡易的な記法で以下のように表すこともできる。
    public string Plain2()
    {
        // 戻り値をstring型として、文字列型を返すことで、内部的にはContentResult型と見なして処理してくれる
        // ※「このようにも書ける」という程度に押さえておけば十分
        return "こんにちは、世界！";
    }

    // p.348 [Add] データベースの内容をカンマ区切りテキストに整形する(CSVファイル)ーーContentヘルパーの応用
    public async Task<IActionResult> Csv()
    {
        var bs = await _db.Books.ToListAsync();
        var data = new StringBuilder();

        // List#ForEachメソッドで取得したデータを繰り返し処理
        bs.ForEach(b => 
            data.Append( 
                // カンマ区切りのテキストに整形
                String.Format($"{b.Isbn},{b.Title},{b.Price},{b.Publisher},{b.Published}\r\n")
            )
        );

        // Content-Disposition応答ヘッダーを追加し、ダウンロード時の既定のファイル名を宣言
        Response.Headers.Append("Content-Disposition", "attachment;filename=data.csv");

        return Content(
                    // カンマ区切りのテキスト
                    data.ToString(), 
                    // コンテンツタイプCSV
                    "text/comma-separated-values", 
                    // Excelでそのまま開けることを意図して文字コードをShift-JISに設定
                    Encoding.GetEncoding("Shift_JIS"));
    }


    // p.350 [Add] 仮想パスで指定されたファイルを出力するーーVirtualFileResultクラス
    // Fileヘルパー(1)の構文
    public IActionResult Image(int id)
    {
        //---------------------------------------------------------------------------------------------------
        //【構文】Fileメソッド
        //  (1) public VirtualFileResult File(string virtualPath, string contentType [, string? fileDownloadName])
        //  (2) public VirtualFileResult File(string virtualPath, string contentType, DateTimeOffset? lastModified [, EntityTagHeaderValue entityTag])
        //   ・virtualPath     ：ファイルの仮想パス
        //   ・contentType     ：ファイルのコンテンツタイプ
        //   ・fileDownloadName：ダウンロード時の既定のファイル名
        //   ・lastModified    ：ファイルの最終更新日時
        //   ・entityTag       ：応答に付与するETag(エンティティタグ)
        //---------------------------------------------------------------------------------------------------

        // wwwroot/imagesフォルダーに、img_301～305.pngを用意しておく
        var path = $"/images/img_{id}.png";
        
        //     File(仮想パス  コンテンツタイプ  既定のファイル名);
      //return File(path,    "image/png",     "sample.png");
        return File(path,    "image/png"); // ※既定のファイル名を省略すると、ファイルをダウンロードするのではなく、ブラウザーにそのまま表示する

        //「～/result/image/301」のようなアドレスでアクセスする
    }

    // p.350 [Add] 物理パスで指定されたファイルを出力するーーPhysicalFileResultクラス
    // Fileヘルパー
    public IActionResult PhysicalImage(int id)
    {
        //-----------------------------------------------------------
        //【構文】PhysicalFileメソッド
        //  ※構文はFileメソッドに準ずる
        //-----------------------------------------------------------

        // C:/data/imagesフォルダーに、img_301～305.pngを用意しておく
        var path = $"C:/data/images/img_{id}.png";
        
        //     File(仮想パス  コンテンツタイプ  既定のファイル名);
        return PhysicalFile(path,    "image/png",     "sample.png");
      //return PhysicalFile(path,    "image/png"); // ※既定のファイル名を省略すると、ファイルをダウンロードするのではなく、ブラウザーにそのまま表示する

        //「～/result/physicalimage/301」のようなアドレスでアクセスする
    }

    // p.350 [Add] ユーザーによるパスしては厳禁ーーパストラバーサル脆弱性
    // PhysicalFileヘルパー
    public IActionResult PathTraversal(string path)
    {
        return PhysicalFile(path, "application/octet-stream");

        //「～/result/pathtraversal?path=C:/data/Program.cs」のようなアドレスでアクセスすると、
        // コンピューター上にあるファイルをダウンロードできてしまう
    }

    // p.355 [Add] プラウザーキャッシュを活用するーー応答ヘッダーEtag、Last-Modified
    // Fileヘルパー(3)の構文
    public IActionResult ImageChache(int id)
    {
        //---------------------------------------------------------------------------------------------------
        //【構文】Fileメソッド
        //  (3) public FirtualFileResult File(string virtualPath, string contentType [, string? fileDownloadName])
        //  (4) public FirtualFileResult File(string virtualPath, string contentType, DateTimeOffset? lastModified [, EntityTagHeaderValue entityTag])
        //   ・virtualPath     ：ファイルの仮想パス
        //   ・contentType     ：ファイルのコンテンツタイプ
        //   ・fileDownloadName：ダウンロード時の既定のファイル名
        //   ・lastModified    ：ファイルの最終更新日時
        //   ・entityTag       ：応答に付与するETag(エンティティタグ)
        //---------------------------------------------------------------------------------------------------
        var path = $"/images/img_{id}.png";
        // File／FileStreamクラスに渡すために、物理パスを生成
        var fullPath = _host.WebRootPath + path;

        return File(
                    virtualPath : path,
                    contentType : "image/png",
                    // 最終更新日  ※System.IO.File.GetLastWriteTimeメソッドでファイルの最終更新日時を取得。
                    //              Controller#Fileメソッドとの競合を避けるために完全修飾名を指定。
                    //            ※GetLastWriteTimeメソッドの戻り値はDateTime型なので、求められた型に合うようにDateTimeOffsetに変換。
                    //             （DateTimeOffsetは、日付情報に加えて、世界協定時からの時差を管理する型）
                    lastModified: new DateTimeOffset( System.IO.File.GetLastWriteTime(fullPath) ),
                    // Etag値(ファイル本体のハッシュ値)
                    // ※ハッシュ値を求めるのは別に用意した自作のComputeSha256メソッド
                    entityTag   : new EntityTagHeaderValue( ComputeSha256(fullPath) )
        );

        //「～/result/imagechache/301」のようなアドレスでアクセスする
    }
    
    // p.355 [Add] プラウザーキャッシュを活用するーー応答ヘッダーEtag、Last-Modified
    // 引数のファイル本体からハッシュ値を求めて返却する
    private static string ComputeSha256(string fullPath)
    {
        // SHA512のインスタンスを生成
        SHA512? sha = SHA512.Create();
        // ファイルパスからFileStreamを生成
        var stream = new FileStream(fullPath, FileMode.Open);
        
        // SHA512#ComputeHashメソッドに対してストリーム(ここではFileStream)を流し込むことで、
        // ファイル本体のハッシュ値を求めることができる。
        byte[] hashArr = sha.ComputeHash(stream);

        // ComputeHashメソッドの戻り値はバイト配列なので、16進数文字列に変換する
        var result = new StringBuilder();
        foreach(byte hash in hashArr)
        {
            // ToString("x2")：数値を16進数文字列に変換する
            result.Append( hash.ToString("x2") );
        }

        // 文字列に変換したうえで「前後をダブルクォートで括る(Etagヘッダーとしてのルール)」
        return $"\"{result.ToString()}\"";

        //--------------------------------------------------------------------------------------------------------------------
        // 【参考】
        //   「SHA-256」(Secure Hash Algorithm 256-bit)とは、どんな長さのデータ（テキスト、ファイルなど）からも
        //   256ビットの固定長で固有のハッシュ値（メッセージダイジェスト）を生成する暗号学的ハッシュ関数です。
        //   生成されたハッシュ値は元のデータに戻すことがほぼ不可能で、データが改ざんされていないか（データの完全性）を確認したり、
        //   電子署名やブロックチェーン(ビットコインなど)、パスワードのハッシュ化など、セキュリティ分野で広く利用されています。 
        //   
        //   ■主な特徴
        //   ・不可逆性  : ハッシュ値から元のデータを復元することは非常に困難です。
        //   ・一意性    : 同じデータからは常に同じハッシュ値が生成されますが、少しでもデータが変わると全く異なるハッシュ値になります。
        //   ・固定長出力: 入力データが1文字でも1GBでも、出力されるハッシュ値は常に256ビット（64文字の16進数表記）です。
        //   ・安全性    : SHA-2ファミリーに属し、強力な衝突耐性（異なるデータから同じハッシュ値が生成されるのを防ぐ能力）を持ちます。 
        //   
        //   ■用途例
        //   ・データ改ざん検出: ファイルをダウンロードした後に、元のハッシュ値と比較して内容が変更されていないかを確認する。
        //   ・パスワード管理  : パスワードそのものではなく、そのハッシュ値を保存することで、万が一漏洩しても元のパスワードがバレにくくする。
        //   ・ブロックチェーン: ビットコインなどの取引記録の正当性を証明する仕組みの中核技術として使われる。 
        //   
        //   まとめると、SHA-256はデータを「指紋」のように変換し、
        //   その指紋を使ってデータの同一性や改ざんの有無を安全に確認するための、非常に重要なセキュリティ技術です。
        //--------------------------------------------------------------------------------------------------------------------
    }


    // p.357 [Add] バイナリ形式のデータを応答するーーFileXxxxxクラス(1)
    public async Task<IActionResult> Photo(int? id)
    {
        //---------------------------------------------------------------------------------------------------
        //【構文】Fileメソッド
        //  (3) public FileContentResult File(byte[] fileContents, string contentType [, string? fileDownloadName 
        //                                                                                [, DateTimeOffset? lastModified 
        //                                                                                 , EntityTagHeaderValue entityTag ]
        //                                                                            ])
        //  (4) public FileStreamResult  File(Stream fileContents, string contentType [, string? fileDownloadName 
        //                                                                                [, DateTimeOffset? lastModified 
        //                                                                                 , EntityTagHeaderValue entityTag ]
        //                                                                            ])
        //   ・fileContents    ：ファイルの内容
        //   ・contentType     ：ファイルのコンテンツタイプ
        //   ・fileDownloadName：ダウンロード時の既定のファイル名
        //   ・lastModified    ：ファイルの最終更新日時
        //   ・entityTag       ：応答に付与するETag(エンティティタグ)
        //---------------------------------------------------------------------------------------------------

        // id値をキーにPhotosテーブルを検索
        var p = await _db.Photos.FindAsync(id);
        // データを取得できなければ「404 Not Found」エラー
        if(p == null) { return NotFound(); }

        // データベースから取得したデータを応答
        return File(p.Content, p.ContentType, p.Name);
        // ※データ本体列「Content(VARBINARY型)」は、.NETの世界ではバイト配列として扱われるので、そのままFileメソッドに渡せる。

        // あまり意味はないが、以下のように書き換えても同じ意味（Streamを受け取る構文）
        //return File(new MemoryStream(p.Content), p.ContentType, p.Name);
    }
    

    // p.358 [Add] iText7で動的にPDFデータを作成するーーFileXxxxxResultクラス(2)
    public IActionResult Pdf()
    {
        // データを一時的に保存するためのメモリ領域を準備
        var stream = new MemoryStream();

        // 新規にPDF文書を生成
        var doc = new iText.Layout.Document(          // Document   ：高レベルな操作を担う
                            new PdfDocument(          // PdfDocument：低レベルな操作を担う
                                new PdfWriter(stream) // PdfWriter  ：コンストラクターで生成する文書の出力先を設定（ここでは一旦メモリに蓄積）
                            )
                );

        // 文字列をフォントを準備
        // itext.font-asianパッケージ標準で利用可能な日本語フォント、文字エンコーディングを設定
        //                                      [ゴシック体]         [横書き]
        var font = PdfFontFactory.CreateFont("HeiseiKakuGo-W5", "UniJIS-UCS2-H");
        doc.SetFont(font);  // setFontメソッドで文書標準のフォントを設定
        // ※日本語を利用する場合は、最低限ここで日本語フォントを設定しておく必要がある。

        // 文字列の出力
        doc.Add(                           // Document.Add：作成したParagraphオブジェクトをAddメソッドで文書に追加
            new Paragraph("こんにちは、")   // Paragraph   ：ブロック(段落)
                    .Add(new Text("世界")  // Text        ：段落に含まれる文字列の断片
                 // Paragraph.Addメソッドで、Paragraphの配下にTextを配置
                             // Textにのみフォントサイズとカラーを設定
                             .SetFontSize(20)
                             .SetFontColor(new DeviceRgb(255, 0, 0)) // 赤字 
                             // カラーはDeviceRgb(int r, int g, int b)クラスで生成（r:赤、g:緑、b:青(0～255)）
                    )
        );

        // ドキュメントをクローズ
        doc.Close();

        // ※この時点では、生成されたPDF文書はメモリ上(MemoryStreamオブジェクト)に一時保存されている状態
        //   ↓
        // 生成したPDF文書を出力
        // ※ToArrayメソッドでバイト配列に変換したうえで、Fileオブジェクトに渡すことで、メモリの内容をクライアントに送出
        return File(stream.ToArray(), MediaTypeNames.Application.Pdf);


        // Fileメソッドの第3引数にファイル名を渡すことで、(生成したPDF文書をブラウザに表示させるのではなく)ダウンロードさせることも可能。
        // return File(stream.ToArray(), MediaTypeNames.Application.Pdf, "sample.pdf");
    }
    
    // p.363 [Add] 別解
    public IActionResult Pdf2()
    {
        // 応答ヘッダーのコンテンツタイプをPDFに設定
        Response.ContentType = MediaTypeNames.Application.Pdf;

        var doc = new iText.Layout.Document(
                            new PdfDocument(
                                // メモリ(MemoryStream)に一旦データを退避させていたものを、
                                // クライアントへの応答ストリーム(Response.Body)に直接書き込み
                                new PdfWriter(Response.Body)
                            )
                );

        var font = PdfFontFactory.CreateFont("HeiseiKakuGo-W5", "UniJIS-UCS2-H");
        doc.SetFont(font);

        doc.Add(
            new Paragraph("こんにちは、")
                    .Add(new Text("世界")
                             .SetFontSize(20)
                             .SetFontColor(new DeviceRgb(255, 0, 0))
                    )
        );

        doc.Close();

        // この場合、アクションが終了した時点で応答も済んでいるので、IActionResult派生オブジェクトも不要。
        // よって、EmptyResultオブジェクトで何もせずに、アクションを終了。
        return Empty;

        // [補足]
        // なお、EmptyResultの一例として、ここでは応答ストリームを直接操作する例を示しましたが、
        // 単体テストを考慮すれば、HTTPに依存するHttpResuponseクラス(Responseオブジェクト)を
        // アクションで利用するのは望ましくない。
        // あくまで、こんな書き方もできる、という引き出しの1つとして捉える。
    }

    // p.368 [Add] IActionResult実装クラスの自作
    public async Task<IActionResult> Output()
    {
        // 自作のIActionResult実装クラスを呼び出し
        return new CsvResult(await _db.Books.ToListAsync());
        // ※ActionResult派生オブジェクトはアクションメソッドの戻り値として呼び出せばよいだけ。
    }

}