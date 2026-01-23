using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SelfAspNetCore.Models;
using SelfAspNetCore.Models.Record;

namespace SelfAspNetCore.Controllers;


// p.261 [Add] LINQ to Entities
public class LinqController : Controller
{
    private readonly MyContext _db;
    // コンストラクター
    public LinqController(MyContext context)
    {
        // コンテキストの注入
        this._db = context;
    }
    // GET: LinqController
    public ActionResult Index()
    {
        return View();
    }
    public IActionResult Basic()
    {
        // // p.261 [Add] クエリ構文（LINQ）
        // // IQueryable<string>? bs
        // var bs = from b in _db.Books
        //          where b.Price < 3000
        //          select b.Title;
        // return View(bs);
        // // p.262 [Add] クエリ構文（LINQ）
        // // interface System.Linq.IQueryable<out T>? bs
        // var bs = from b in _db.Books
        //          where b.Price < 3000
        //          // 複数のプロパティを取得したい場合には、匿名型を利用して以下のように表現
        //          select new { b.Title, b.Price }; 
        // return View(bs);
        // p.263 [Add] メソッド構文（LINQ）
        // interface System.Linq.IQueryable<out T>? bs
        var bs = _db.Books
                 .Where(b => b.Price < 3000)
                 .Select(b => b.Title);
        return View(bs);
    }
    // p.265 [Add] LIKE演算子に相当するContainsメソッド（LINQメソッド構文）
    public IActionResult Contains()
    {
        var bs = _db.Books
                 .Where(b =>  b.Title.Contains("JavaScript"));
               // ※含んでいない場合は、否定演算子（!）を利用する
               //.Where(b => !b.Title.Contains("JavaScript"));
        return View("Items", bs);
    }
    // p.265 [Add] 前方一致／後方一致検索をするStartsWith／EndsWithメソッド（LINQメソッド構文）
    public IActionResult StartsWith()
    {
        var bs = _db.Books
                 .Where(b => b.Title.StartsWith("独習"));
               //.Where(b => b.Title.EndsWith("独習"));
        return View("Items", bs);
    }
    // p.266 [Add] IN演算子に相当するArrayクラスのContainsメソッド（LINQメソッド構文）
    public IActionResult Selection()
    {
        var bs = _db.Books
                 .Where(b => new int[] {3, 9}.Contains(b.Published.Month));
        
        return View("List", bs);
    }
    // p.266 [Add] BETWEEN演算子に相当する操作（LINQメソッド構文）
    // ※LINQではこれを直接表現するメソッドがないため、比較演算子と論理演算子の組み合わせで表現する
    public IActionResult Between()
    {
        var bs = _db.Books
                .Where(b => 4000<=b.Price && b.Price<=4500 );
        return View("Items", bs);
    }
    // p.267 [Add]【別解】Whereメソッドを列記。この場合、And連結される。
    public IActionResult BetweenAnd()
    {
        var bs = _db.Books
                .Where(b => b.Price>=4000 )
                .Where(b => b.Price<=4500 );
        return View("Items", bs);
    }
    // p.267 [Add] 正規表現検索（LINQメソッド構文）
    // ※正規表現をデータベースで扱うことはできないので、正規表現そのものはアプリ側で処理する
    public IActionResult Regex()
    {
        // 数字を含む正規表現
        var reg = new Regex("\\d");
        var bs = _db.Books
                // AsEnumerableメソッドでIEnumerableオブジェクトに変換し、
                .AsEnumerable()
                // それを正規表現で検索している  
                .Where(b => reg.IsMatch(b.Title))
                .ToList();
        //-----------------------------------------------------------------------
        //【注意】
        // 大量のデータを処理する場合にはパフォーマンス劣化の原因になる点に注意
        // コンソールログのSQLを見てもWHERE句がない（すべてのデータを一旦取り出している）
        //
        // 可能であれば、データベース側で対象のデータを最大限絞り込んでから
        // (AsEnumerable前にできることを済ませてから)、正規表現に引き渡すようにする
        //-----------------------------------------------------------------------
        return View("List", bs);
    }
    // p.268 [Add] Whereメソッドの特殊系として、SingleAsyncメソッド（LINQメソッド構文）
    // ※指定された条件式に合致した単一のデータを取得する
    public async Task<IActionResult> SingleAsync()
    {
        var bs = await _db.Books
                .SingleAsync(b => b.Isbn == "978-4-7981-8094-6");
                // 0件、複数件取得の場合はエラーを発生（InvalidOperationException）
                // 結果が0件の場合、方に応じた既定値（参照型ではnull）を返したい場合は、SingleOrDefaultAsyncメソッドを利用する
        return Content(bs.Title);  // 結果：独習Java
    }
    public IActionResult Single()
    {
        var bs = _db.Books
                .Single(b => b.Isbn == "978-4-7981-8094-6");
        return Content(bs.Title);  // 結果：独習Java
    }
    // p.267 [Add] Or連結する場合、1つのWhereメソッドで表す
    public IActionResult Or()
    {
        var bs = _db.Books
                .Where(b => 4000==b.Price || b.Price==4400 );
        return View("Items", bs);
    }
    // p.269 [Add] 特定の条件でデータが存在するかどうかを確認するAnyAsync／AllAsyncメソッド（LINQメソッド構文）
    // ※条件に合致するデータの存在確認
    public async Task<IActionResult> Exists()
    {
        var result = await _db.Books
                       // 条件に合致するデータが1件でも存在するか
                       .AnyAsync(b => b.Price >= 4000);  // 結果：True
                       // すべてのデータが条件に合致するか
                     //.AllAsync(b => b.Price >= 4000);  // 結果：False
        return Content(result.ToString());
    }
    // p.269 [Add] 指定された条件（タイトルと刊行日）で書籍情報を絞り込むための例（LINQメソッド構文）
    // モデルバインド：GETパラメータ keyword, released を引数にバインドする
    public IActionResult Filter(string keyword, bool? released)
    {
        // Booksテーブルを全件取得
        var bs = _db.Books.Select(b => b);
        // [キーワード]欄が空でない場合、
        if (!string.IsNullOrEmpty(keyword))
        {
            // [キーワード]で部分一致検索をして、bsに再代入
            bs = bs.Where(b => b.Title.Contains(keyword));
        }
        // [刊行済み]がチェック状態の場合、今日以前の刊行日の書籍で絞り込み
        if(released.HasValue && released.Value)
        {
            // 今日以前の刊行日で絞り込んで、bsに再代入
            bs = bs.Where(b => b.Published <= DateTime.Now);
        }
        // ※Where句を列記した場合はAnd連結となる
        //   また、このような複合的な検索条件の組み立てが可能であるのも遅延実行の恩恵
        return View(bs);
    }
    // p.272 [Add] ソート可能なグリッド表を作成する（LINQメソッド構文）
    public IActionResult SortGrid(string sort)
    {
        // ソートキー／順序を識別するためのキー文字列を設定
        ViewBag.Isbn      = sort == "Isbn"             ? "dIsbn"      : "Isbn";
        ViewBag.Title     = string.IsNullOrEmpty(sort) ? "dTitle"     : "";
        ViewBag.Price     = sort == "Price"            ? "dPrice"     : "Price";
        ViewBag.Publisher = sort == "Publisher"        ? "dPublisher" : "Publisher";
        ViewBag.Published = sort == "Published"        ? "dPublished" : "Published";
        ViewBag.Sample    = sort == "Sample"           ? "dSample"    : "Sample";
        // Booksテーブルから全件取得
        var bs = _db.Books.Select(b => b);
        // 渡されたキー文字列に基づいてソート式を選択し、bsに再代入
        bs = sort switch  // switch式
                {
                    // 昇順
                    "Isbn"      => bs.OrderBy(b => b.Isbn),
                    "Title"     => bs.OrderBy(b => b.Title),
                    "Price"     => bs.OrderBy(b => b.Price),
                    "Publisher" => bs.OrderBy(b => b.Publisher),
                    "Published" => bs.OrderBy(b => b.Published),
                    "Sample"    => bs.OrderBy(b => b.Sample),
                    // 降順
                    "dIsbn"      => bs.OrderByDescending(b => b.Isbn),
                    "dTitle"     => bs.OrderByDescending(b => b.Title),
                    "dPrice"     => bs.OrderByDescending(b => b.Price),
                    "dPublisher" => bs.OrderByDescending(b => b.Publisher),
                    "dPublished" => bs.OrderByDescending(b => b.Published),
                    "dSample"    => bs.OrderByDescending(b => b.Sample),
                    // デフォルト
                    _ => bs.OrderBy(b => b.Title)
                };
        return View(bs);
    }
    // p.274 [Add] 特定のプロパティだけを取得するSelectメソッド（LINQメソッド構文）
    public IActionResult Select()
    {
        var bs = _db.Books
                .OrderByDescending(b => b.Published)
                .Select(b => 
                    new SummaryBookView( // ←モデルビュー（SelfAspNetCore.Models.SummarBookView）
                        b.Title.Substring(0, 7) + "...",
                        (int) (b.Price * 0.9),
                        b.Published <= DateTime.Now ? "発売中" : "発売予定"
                    )
                );
        
        return View(bs);
    }
    // p.277 [Add] 特定範囲のデータを取り出すSkip／Takeメソッド（LINQメソッド構文）
    //   Skipメソッド：指定された件数だけデータを読み飛ばし
    //   Takeメソッド：指定された件数だけを取得
    // ※Skip／Takeメソッドを組み合わせることで、m～n件目のデータを抜き出すといった操作も可能
    public IActionResult Skip()
    {
        // 書籍情報を刊行日の昇順に並べたときに、3～5件目を取得
        var bs = _db.Books
                .OrderBy(b => b.Published)
                .Skip(2)  // 2件スキップ
                .Take(3); // スキップ後から3件取得
        return View("List", bs);
    }
    // p.278 [Add] ページング機能を実装する（Skip／Takeメソッドの応用）
    // アクセス「/linq/page」「/linq/page/2」
    public IActionResult Page(int id = 1)
    {
        int pageSize = 3;      // ページあたりの表示件数
        int pageNum = id - 1;  // 現在のページ番号（先頭のページ番号は0）
        var bs = _db.Books
                .OrderBy(b => b.Published)
                .Skip(pageSize * pageNum)  // データの開始位置
                .Take(pageSize);           // 取得する件数はページサイズ
        return View("List", bs);
    }
    // p.279 [Add] 先頭／末尾の1件だけを取得するFirstAsync／LastAsyncメソッド
    public async Task<IActionResult> First()
    {
        var bs = await _db.Books
                .OrderBy(b => b.Published)
                .FirstAsync();
        return View("Details", bs);
    }
    // p.279 [Add] データをグループ化するGroupByメソッド（LINQメソッド構文）
    public IActionResult Group()
    {
        var bs = _db.Books
                .GroupBy(b => b.Publisher)
                .OrderBy(group => group.Key);
        return View(bs);
    }
    // p.279 [Add] レコードを使用し、Title／Priceプロパティだけを含んだオブジェクトを返す（GroupByメソッド）
    public IActionResult GroupMini()
    {
        var bs = _db.Books
                .GroupBy(
                    b => b.Publisher,
                    b => new MiniBook(b.Title, b.Price)
                )
                .OrderBy(group => group.Key);
        return View(bs);
    }
    // p.279 [Add] レコードを使用し、複数列をキーにグループ化する（GroupByメソッド）
    public IActionResult GroupMulti()
    {
        var bs = _db.Books
                .OrderBy(b => b.Publisher)
                .ThenBy(b  => b.Price)
                .GroupBy(b => new BookGroup(
                        b.Publisher,      // 出版社
                        b.Published.Year  // 出版日(年)
                    )
                );
        //---------------------------------------------------------
        // なお、以下のようなキー設定も可能。
        // ・b.Title[0]  ：書名の先頭文字でグループ化
        // ・b.Price/1000：0～999、1000～1999...の価格帯でグループ化
        //---------------------------------------------------------
        return View(bs);
    }
    
    // p.283 [Add] Having句に相当する「GroupBy＋Where」メソッド（LINQメソッド構文）
    // ※グループ化した結果を、さらに条件で絞り込む
    public IActionResult Having()
    {
        var bs = _db.Books
                .GroupBy(b => b.Publisher)                              // Group By句  ：出版社
                .Where(group => group.Average(b => b.Price) >= 3000)    // Having句相当：出版社ごとの平均価格が3000円以上
                .Select(group => new HavingBook(
                                    group.Key,                          // Select句1   ：グループ化キー（出版社）
                                    (int) group.Average(b => b.Price)   // Select句2   ：出版社ごとの平均価格
                                )
                        );
        return View(bs);
    }
    
    // p.284 [Add] グループ化による結果でソートする「GroupBy＋OrderBy」メソッド（LINQメソッド構文）
    public IActionResult HavingSort()
    {
        var bs = _db.Books
                .GroupBy(b => b.Publisher)
                .OrderBy(group => group.Average(b => b.Price))
                .Select( group => new HavingBook(
                                    group.Key,
                                    (int)group.Average(b=>b.Price)
                                )
                        );
        return View("Having", bs);
    }
    // p.285 [Add] エンティティ同士を結合するJoinメソッド（LINQメソッド構文）
    public IActionResult Join()
    {
     // var = IEnumerable<BookReviewView>
        var bs = _db.Books              // 結合元のデータソース
                 .Join( _db.Reviews,    // 結合先のデータソース
                     b => b.Id,         // 結合元のキー
                     rev => rev.BookId, // 結合先のキー
                     (b, rev) => new BookReviewView(b.Title, rev.Body) // 結合結果から特定のプロパティを取り出すための関数
                 );
        
        //----------------------------------------------------------------------------------------------------------------
        //【Joinメソッドの構文】
        // ・TResult：結果要素の型
        // ・TOuter ：結合する最初の要素の型
        // ・TInner ：結合する2番目の要素の型
        // ・TKey   ：結合キーの型
        //
        // public IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult> (
        //              IEnumerable<TInner>         inner,             // 結合先のデータソース
        //              Func<TOuter,TKey>           outerKeySelector,  // 結合元のキー
        //              Func<TInner,TKey>           innerKeySelector,  // 結合先のキー
        //              Func<TOuter,TInner,TResult> resultSelector     // 結合結果から特定のプロパティを取り出すための関数
        //          )
        //----------------------------------------------------------------------------------------------------------------
        return View(bs);
    }
    // p.288 [Add] 複数のレコードをまとめて更新／削除する
    //  更新：ExecuteUpdateAsyncメソッド
    //  削除：ExecuteDeleteAsyncメソッド
    public async Task<IActionResult> Update()
    {
        // // Publisher列が「翔泳社」である書籍情報に対して、Price列を一律に2割引きにする
        // // ↓これまでの知識で対応した場合
        // foreach(var b in _db.Books.Where(b => b.Publisher == "翔泳社") )
        // {
        //     b.Price = (int) (b.Price * 0.8);
        // }
        // await _db.SaveChangesAsync();
        // ⇒更新対象の件数分だけUPDATE命令が発行されてしまう。
        // Publisher列が「翔泳社」である書籍情報に対して、Price列を一律に2割引きにする
        // ExecuteUpdateAsyncメソッドを使用して複数レコードをまとめて更新
        await _db.Books
             .Where(b => b.Publisher == "翔泳社")
             .ExecuteUpdateAsync(
                 setter => setter
                             .SetProperty(
                                 b => b.Price,              // 更新対象の列
                                 b => (int) (b.Price * 0.8) // 更新値
                             )
                             // ※複数の列を更新する場合、SetPropertyメソッドを列記する
             );
        // // ExecuteDeleteAsyncメソッドを使用して複数レコードをまとめて削除する場合
        // await _db.Articles
        //      .Where(a => a.Category == "JavaScript")
        //      .ExecuteDeleteAsync();
        return Content("更新しました。");
    }
    // p.290 [Add] エンティティの関係を追加する
    // ※いずれも新規のBook／Reviewエンティティを挿入
    public async Task<IActionResult> Insert()
    {
        // いずれも新規のBook／Reviewエンティティを挿入するのであれば、これまで通りAddメソッドでエンティティを追加するだけ。
        _db.Reviews.Add(
                // Reviewエンティティ
                new Review {
                        Name = "藤井友美",
                        Body = "しっかり勉強したい人向けの本です。最初に...",
                        LastUpdated = new DateTime(2024, 05, 17),
                        // Reveiwエンティティの参照オブジェクトであるBookプロパティを入れ子で設定
                        Book = new Book
                        {
                            Isbn = "978-4-7981-6849-4",
                            Title = "独習PHP",
                            Price = 3740,
                            Publisher = "翔泳社",
                            Published = new DateTime(2021, 06, 14),
                            Sample = true
                        }
                    }
                );
        await _db.SaveChangesAsync();
        return Content("データを追加しました。");
    }
    // p.290 [Add] エンティティの関係を追加する
    // ※既存の書籍情報に、新規のReviewエンティティを挿入
    public async Task<IActionResult> Insert2()
    {
        // Reviewを追加する対象の書籍情報を先に取得しておく（プリンシパルエンティティ）
        var book = await _db.Books.FindAsync(1);
        // 既存の書籍情報を取得しておいて、新たに作成したReviewオブジェクトの依存ナビゲーション(Book)に設定
        _db.Reviews.Add(
                // Reviewエンティティ
                new Review {
                        Name = "木村裕二",
                        Body = "最近は、意外と書き方が変わっていて勉強になった。",
                        LastUpdated = new DateTime(2024, 06, 03),
                        // 取得しておいた書籍情報を依存ナビゲーション(Book)に設定
                        Book = book!,
                        // 以下のように外部キープロパティに対して直接主キーを渡してもほぼ同じ意味
                        // BookId = 1
                    }
                );
        await _db.SaveChangesAsync();
        return Content("データを追加しました。");
    }
    // p.291 [Add] エンティティの関係を更新する
    // ※既存の関連を更新するのも、同じ要領で可能。
    public async Task<IActionResult> associate()
    {
        // 更新対象のBookデータとReviewデータをそれぞれ取得
        var book = await _db.Books.FindAsync(1);
        var review = await _db.Reviews.FindAsync(7);
        // 取得しておいた書籍情報を依存ナビゲーション(Book)に設定
        review!.Book = book!;
        // 以下のように外部キープロパティに対して直接主キーを渡してもほぼ同じ意味
        // review!.BookId = 1;
        await _db.SaveChangesAsync();
        return Content("データを更新しました。");
    }
    // p.291 [Add] エンティティの関係を削除する
    public async Task<IActionResult> Delete()
    {
        // // 以下のようなコードはエラーとなる
        // var b = await _db.Books.FindAsync(1);
        // _db.Books.Remove(b);
        // await _db.SaveChangesAsync();
        // ⇒ ※外部キー参照制約で、関連するレビュー(子レコード)が存在するため削除できない。
        //      これを動作させるには、Review.csの「参照ナビゲーション＋外部キープロパティ」をNull許容型に変更する。
        //      (参照先の書生情報を削除したときも、紐づいたレビュー情報は残したいという場合など。)
        // Includeメソッドを利用して、目的のBookエンティティと、これに関連づいたReviewエンティティを取得
        var b = await _db.Books
            .Include(b => b.Reviews)
            .SingleAsync(b => b.Id == 1);
        // 対象データをメモリ上から削除
        _db.Books.Remove(b);
        // 対象データをDB上から削除
        await _db.SaveChangesAsync();
        return Content("データを削除しました。");
    }

    // p.295 [Add] EF Coreの標準のトランザクション処理
    public async Task<IActionResult> Transaction()
    {
        _db.Books.Add(
            new Book
            {
                Isbn = "978-4-297-13919-3",
                Title = "3ステップで学ぶMySQL入門",
                Price = 2860,
                Publisher = "技術評論社",
                Published = new DateTime(2024, 01, 25),
                Sample = true
            }
        );
        _db.Books.Add(
            new Book
            {
                Isbn = "978-4-7981-8094-6",
                // Not Null制約に違反
                // Title =  null!,
                Title = "独習Java 第6版",
                Price = 3960,
                Publisher = "翔泳社",
                Published = new DateTime(2024, 02, 15),
                Sample = true
            }
        );

        // SaveChangesAsyncメソッドによる更新を1つのトランザクションとして扱うため、
        // このタイミングでエラーとなりロールバックされる。
        await _db.SaveChangesAsync();
        return Content("データを追加しました。");
    }

    // p.296 [Add] 明示的なトランザクション
    public async Task<IActionResult> Transaction2()
    {
        // トランザクション開始
        using(var tx = _db.Database.BeginTransaction())
        {
            try
            {
               _db.Books.Add(
                    new Book
                    {
                        Isbn = "978-4-297-13919-3",
                        Title = "3ステップで学ぶMySQL入門",
                        Price = 2860,
                        Publisher = "技術評論社",
                        Published = new DateTime(2024, 01, 25),
                        Sample = true
                    }
                );

                // SaveChangesAsyncメソッドで、1件登録
                await _db.SaveChangesAsync();

                // ExecuteUpdateAsyncメソッドで、まとめて更新
                await _db.Books
                    // 更新条件：出版社が"翔泳社"
                    .Where(b => b.Publisher == "翔泳社")
                    .ExecuteUpdateAsync(
                                    setter =>
                                        setter.SetProperty(
                                            b => b.Price,               // 価格列を、
                                            b => (int) (b.Price * 0.8)  // 2割引きに更新
                                        )
                    );

                // 一連の処理をコミット
                tx.Commit();
                return Content("データベース処理が正常終了しました。");
            } 
            catch(Exception)
            {
                // 一連の処理をロールバック
                tx.Rollback();
                return Content("データベース処理に失敗しました。");
            }
        }
    }

    // p.305 [Add] エンティティを操作した前後の処理を実装する（インタセプタ―）
    public async Task<IActionResult> AddUp()
    {
        _db.Articles.Add(new Article
        {
            Title    = "Next.jsの新しい概念を学ぶ",
            Url      = "https://codezine.jp/article/corner/970",
            Category = "JavaScript"
        });

        foreach (var a in _db.Articles.Where(a => a.Category == "Rails"))
        {
            a.Category = "Ruby on Rails";
        }

        await _db.SaveChangesAsync();
        return Content("データを追加＆更新しました。");
    }
}
    
 
