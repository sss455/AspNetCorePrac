using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SelfAspNetCore.Models;

namespace SelfAspNetCore.Controllers;

// p.72 [Auto] dotnet aspnet-codegenerator（Scaffolding機能）で自動生成
public class BooksController : Controller
{
    private readonly MyContext _context;

    // コンストラクター
    public BooksController(MyContext context)
    {
        // コンテキストの注入
        _context = context;
    }

    // GET: Books
    // 一覧画面表示：Indexアクションを非同期化し、データベースにアクセス
    public async Task<IActionResult> Index()
    {
        // データベースから取得した結果をテンプレートに送信
        return View(await _context.Books.ToListAsync());
    }

    // GET: Books/Details/5
    // 詳細画面表示：リクエストデータidを受け取る
    public async Task<IActionResult> Details(int? id)
    {
        // idが無指定、書籍情報が得られない場合
        if (id == null)
        {
            // 404エラー
            return NotFound();
        }
        // 引数idをキーにBooksテーブルを検索
        var book = await _context.Books
            .FirstOrDefaultAsync(m => m.Id == id);
        // データが見つからなかった場合は404エラー
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    // GET: Books/Create
    // 登録画面表示
    public IActionResult Create()
    {
        return View();
    }

    // POST: Books/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    // [翻訳]
    // オーバーポスティング攻撃(過多ポスティング攻撃)から保護するため、バインドしたい特定のプロパティを有効にしてください。
    // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
    //
    // 新規登録画面：HTTP POSTに割り当てられたアクション(Createボタン押下時)
    [HttpPost]
    [ValidateAntiForgeryToken] // p.416 CSRF対策のためにワンタイムトークンのチェックを行う標準のフィルター属性
    // ポストデータを引数にバインド        ※クロスサイトリクエストフォージェリ脆弱性
    public async Task<IActionResult> Create(
        // オーバーポスティング攻撃対策のため、引数Bookエンティティ(モデルオブジェクト)にバインドしてよいプロパティを明示的に列挙
        [Bind("Id, Isbn, Title, Price, Publisher, Published, Sample")] Book book)
    {
        if (ModelState.IsValid)
        {
            //-------------------------------------
            // エンティティをデータベースに反映
            //-------------------------------------
            // Addメソッドでエンティティをコンテキストに追加（メモリ上の登録操作。まだデータベースは変更されない）
            // 複数のデータをまとめて登録したいならば、Addメソッドを複数回呼び出す
            _context.Add(book);
            // SaveChangesAsyncメソッドでデータベースへ反映（実操作）
            await _context.SaveChangesAsync();
            // 処理に成功したら、一覧画面にリダイレクト
            return RedirectToAction( nameof(Index) );
        }
        // 入力に問題がある場合は登録フォームを再描画
        return View(book);
    }

    // GET: Books/Edit/5
    // 編集画面表示：[Edit]リンクから呼び出され、編集フォームを生成
    public async Task<IActionResult> Edit(int? id)
    {
        // 引数id、または書籍情報が空の場合、404 Not Foundエラー
        if (id == null)
        {
            return NotFound();
        }
        // 引数idをキーにテーブルを検索
        var book = await _context.Books.FindAsync(id);
        // 対応する書籍情報が存在しなければ、404 Not Foundエラー
        if (book == null)
        {
            return NotFound();
        }
        
        //---------------------------------------
        // p.122 [Add] Selectタグヘルパー
        //---------------------------------------
        // テーブルから重複のない出版社名を取得
        var list = _context.Books
                    .Select(b => new { Publisher = b.Publisher } )
                    .Distinct();
        // 選択オプションのリストを準備
        ViewBag.Opts = new SelectList(list, "Publisher", "Publisher");
        // 編集フォームを表示
        return View(book);
    }

    // GET: Books/Edit2/5
    // 編集画面表示：[Edit2]リンクから呼び出され、編集フォームを生成
    public async Task<IActionResult> Edit2(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        var list = _context.Books
                    .Select(b => new { Publisher = b.Publisher } )
                    .Distinct();
        // 選択オプションのリストを準備
        //ViewBag.Opts = new SelectList(list, "Publisher", "Publisher");
        // p.277 [Add] ビューモデルに設定
        SelectView selectView = new SelectView {
                                    Book = book, 
                                    Publishers = new SelectList(list, "Publisher", "Publisher")
                                };
        // 編集フォームを表示
        return View(selectView);
    }

    // POST: Books/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    // [翻訳]
    // オーバーポスティング攻撃(過多ポスティング攻撃)から保護するため、バインドしたい特定のプロパティを有効にしてください。
    // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
    //
    // 編集画面：[Save]ボタンで編集内容をデータベースに反映
    [HttpPost]
    [ValidateAntiForgeryToken]
    // ポストデータを引数にバインド
    // public async Task<IActionResult> Edit(int id, 
    //                                     // オーバーポスティング攻撃対策のため、引数Bookエンティティ(モデルオブジェクト)にバインドしてよいプロパティを明示的に列挙
    //                                     [Bind("Id,Isbn,Title,Price,Publisher,Published,Sample")] Book book)
    // p.297 [Mod] オプティミスティック(楽観的)同時実行制御
    public async Task<IActionResult> Edit(int id, 
                                         // p.308 標準的な検証機能の実装
                                         // ※エンティティに定義した検証はモデルバインドのタイミングで自動的に実施
                                         [Bind("Id,Isbn,Title,Price,Publisher,Published,Sample,RowVewsion")] Book book)
    {
        // 隠しフィールドのid値と、ルートパラメータのidとが一致するか
        if (id != book.Id)
        {
            return NotFound();
        }
        // p.308 標準的な検証機能の実装
        // 検証に成功したらデータベース処理＆リダイレクト
        // ※検証が成功したかどうかをチェックし、何かしらのエラーが存在する場合にはビューにフィードバックする
        if (ModelState.IsValid)
        {
            try
            {
                //-----------------------------------------
                // エンティティの更新ををデータベースに反映
                //-----------------------------------------
                // Updateメソッドで、このエンティティが更新されたことをコンテキストに通知（メモリ上の更新操作。まだデータベースは変更されない）
                _context.Update(book);
                //_context.Entry(book).State = EntityState.Modified; // Stateプロパティを利用。↑と同じ意味
                // SaveChangesAsyncメソッドでデータベースへ反映（実操作）
                await _context.SaveChangesAsync();
            }
            // 競合が発生した場合の処理
            catch (DbUpdateConcurrencyException)
            {
                // 該当の書籍が存在しなければ、404 Not Fountエラー
                // ※他のユーザーが先に削除してしまった場合
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                // 書籍が存在する場合は競合エラー
                // ※他のユーザーが同じデータを更新してしまった場合
                else
                {
                    // 例外を再スロー
                    // throw;
                    // p.297 [Mod] オプティミスティック(楽観的)同時実行制御
                    ModelState.AddModelError(string.Empty, "競合が検出されました。");
                    //--------------------------------------------------------------------------------
                    // アクションで発生したエラーを管理するのは、ModelStateDictionaryオブジェクト
                    // Controller#ModelStateプロパティでアクセスできる
                    //【構文】
                    //  void AddModelError(string key, string errormessage)
                    //   ・key         ：キー
                    //   ・errorMessage：エラーメッセージ
                    // ※引数keyには、エラーの発生元を示すプロパティを指定する。
                    //   しかし、この例ではページレベルのエラーなので、ダミーの値として空文字列を渡しておく。
                    //--------------------------------------------------------------------------------
                    return View(book);
                }
            }
            // データベース更新に成功した場合はリダイレクト
            return RedirectToAction(nameof(Index));
        }
        // p.308 標準的な検証機能の実装
        // 入力に問題がある場合は編集フォームを再描画
        return View(book);
    }

    // GET: Books/Delete/5
    // [Delete]リンクによって呼び出され、削除画面を生成
    public async Task<IActionResult> Delete(int? id)
    {
        // 引数idが空の場合は404 Not Foundエラー
        if (id == null)
        {
            return NotFound();
        }
        // 引数idをキーに書籍情報を取得
        var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);
        // 合致する書籍が存在しなければ404 Not Foundエラー
        if (book == null)
        {
            return NotFound();
        }
        // 削除画面を表示
        return View(book);
    }

    // POST: Books/Delete/5
    // [Delete]ボタンで削除処理を実行
    //   ※引数が一致する同名メソッドは定義できないため、アクションメソッド名をDeleteConfirmedとし、
    //     ActionName属性でアクション名をDeleteとすることで、1つのDeleteアクションとしてまとめている。
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // 引数idをキーに書籍情報を取得
        var book = await _context.Books.FindAsync(id);
        // 合致する書籍が存在したら削除（存在しなければ何もしない）
        if (book != null)
        {
            // Removeメソッドで、このエンティティが削除されたことをコンテキストに通知（メモリ上の更新操作。まだデータベースは変更されない）
            _context.Books.Remove(book);
        }
        // SaveChangesAsyncメソッドでデータベースへ反映（実操作）
        await _context.SaveChangesAsync();
        // 削除後は一覧にリダイレクト
        return RedirectToAction(nameof(Index));
    }

    // 指定された書籍が存在するかを判定
    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }

    // GET: Books/Details2/5
    // p.131 [Add] DisplayForModelメソッド：モデル単位の出力を生成する
    // 詳細画面2表示
    public async Task<IActionResult> Details2(int? id)
    {
        // idが無指定、書籍情報が得られない場合、404エラー
        if (id == null) return NotFound();
        // 引数idをキーにBooksテーブルを検索
        var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);
        // データが見つからなかった場合は404エラー
        if (book == null)　return NotFound();
        return View(book);
    }

    // p.320 [Add] サーバーサイドと連携した検証を実装するーー Remote検証
    // 非同期でISBNの重複チェックを行う
    public async Task<IActionResult> UniqueIsbn(string isbn)
    // Titleプロパティも送信したい場合、AddtionalFieldsプロパティを使用して以下のように記述。
    //[Remote("UniqueIsbn", "Books", AdditionalFields=nameof(Title))] 
    //この場合は追加のプロパティも受け取れるように、アクションの引数も追加しておく（引数の名前はプロパティと同名）
    //public async Task<IActionResult> UniqueIsbn(string isbn, string title)
    {
        // 入力されたISBNと同じISBNが既に登録されているか（重複チェック）
        if(await _context.Books.AnyAsync(b => b.Isbn == isbn))
        {
            // 戻り値はJSON形式でなければならない
            // JSONメソッド(IActionResult型の値を返すヘルパメソッドの一種)を使用し、
            // JSON形式に変換した値を返却する。
            return Json("このISBNコードはすでに登録されています。");
        }
        
        // 問題ない場合、JSON形式でtrueを返す
        return Json(true);
    }
}

