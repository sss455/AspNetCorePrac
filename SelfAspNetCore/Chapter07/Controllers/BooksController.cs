using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chapter07.Models;
using Chapter07.Models.Repositories;

namespace Chapter07.Controllers;

public class BooksController : Controller
{
    // DBコンテキスト
    private readonly MyContext _context;

    // p.448 [Add] 自作サービスの登録
    // [4] アクションを修正
    // リポジトリクラス
    private readonly IBookRepository _repo;


    // コンストラクター
    public BooksController(MyContext context, IBookRepository repo)
    {
        _context = context;

        // p.448 [Add] 自作サービスの登録
        // [4] アクションを修正
        // ※注入したいサービスは、Publicなコンストラクターの引数として受け取る。
        _repo = repo;
    }


    // 一覧画面表示
    // GET: Books
    public async Task<IActionResult> Index()
    {
        // return View(await _context.Books.ToListAsync());
        // p.448 [Mod] 自作サービスの登録
        // [4] アクションを修正
        // ※リポジトリ経由で書籍情報を取得
        return View( await _repo.GetAllAsync() );
    }


    // 詳細画面表示
    // GET: Books/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var book = await _context.Books
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }


    // 新規作成画面表示
    // GET: Books/Create
    public IActionResult Create()
    {
        return View();
    }
    
    // POST: Books/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Isbn,Title,Price,Publisher,Published,Sample,RowVewsion")] Book book)
    {
        if (ModelState.IsValid)
        {
            // _context.Add(book);
            //await _context.SaveChangesAsync();
            // p.448 [Mod] 自作サービスの登録
            // [4] アクションを修正
            // ※リポジトリ経由で書籍情報を登録
            await _repo.CreateAsync(book);

            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }
    
    // 編集画面表示
    // GET: Books/Edit/5
    public async Task<IActionResult> Edit(int? id)
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
        return View(book);
    }
    
    // POST: Books/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Isbn,Title,Price,Publisher,Published,Sample,RowVewsion")] Book book)
    {
        if (id != book.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }
    

    // 削除画面表示
    // GET: Books/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var book = await _context.Books
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }
    
    // 削除
    // POST: Books/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    
    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }
}