using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using SelfAspNet.Models;

namespace SelfAspNet.Controllers
{
    public class BooksController : Controller
    {
        private readonly MyContext _context;

        public BooksController(MyContext context)
        {
            _context = context;
        }

        // private readonly IBookRepository _rep;
        // public BooksController(IBookRepository rep, MyContext context)
        // {
        //     _rep = rep;
        //     _context = context;
        // }

        // public BooksController(IServiceProvider prov)
        // {
        //     _rep = prov.GetRequiredService<IBookRepository>();
        // }

        public async Task<IActionResult> UniqueIsbn(string isbn)
        // public async Task<IActionResult> UniqueIsbn(string isbn, string title)
        {
            if (await _context.Books.AnyAsync(b => b.Isbn == isbn))
            {
                return Json("ISBNコードは既に登録されています。");
            }
            return Json(true);
        }
        // GET: Books
        public async Task<IActionResult> Index()
        {
            ViewBag.Success = TempData["Success"];
            return View(await _context.Books.ToListAsync());

            // return View(await _rep.GetAllAsync());
        }

        // GET: Books/Details/5
        // [ResponseCache(Duration = 60)]
        // [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "mode" })]
        // [ResponseCache(Duration = 60, VaryByHeader = "User-Agent")]
        // [ResponseCache(CacheProfileName = "MyCache")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            // var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

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
        public async Task<IActionResult> Create([Bind("Id,Isbn,Title,Price,Publisher,Published,Sample")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();

                // await _rep.CreateAsync(book);
                TempData["Success"] = $"「{book.Title}」を登録しました。";
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

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

            // var list = _context.Books
            //     .Select(b => new { Publisher = b.Publisher })
            //     .Distinct();

            // ViewBag.Opts = new SelectList(
            //     list, "Publisher", "Publisher");
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [HttpPut]
        // [AcceptVerbs("Post", "Put")]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,Isbn,Title,Price,Publisher,Published,Sample,RowVersion")] Book book)
        public async Task<IActionResult> Edit(int id, [Bind("Id,Isbn,Title,Price,Publisher,Published,Sample")] Book book)
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
                        // ModelState.AddModelError(string.Empty, "競合が検出されました。");
                        // return View(book);
                    }
                }
                return RedirectToAction(nameof(Index));

                // return RedirectToAction("Index", "Hello");
                // return RedirectToAction(nameof(Details), new { id = 108, charset="utf-8" });
                // return RedirectToAction(nameof(Index), null, "supplement");
                // return RedirectToRoute("default", new { controller = "Books",
                //     action = "Details", id = 108, charset="utf-8" });

            }
            return View(book);
        }

        // TryUpdateModelAsyncでの書換
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id)
        // {
        //     var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);

        //     if (book == null)
        //     {
        //         return NotFound();
        //     }
        //     if (await TryUpdateModelAsync(book, "", b => b.Isbn, b => b.Title,
        //         b => b.Price, b => b.Publisher, b => b.Published, b => b.Sample))
        //     {
        //         if (ModelState.IsValid)
        //         {
        //             try
        //             {
        //                 _context.Update(book);
        //                 await _context.SaveChangesAsync();
        //             }
        //             catch (DbUpdateConcurrencyException)
        //             {
        //                 if (!BookExists(book.Id))
        //                 {
        //                     return NotFound();
        //                 }
        //                 else
        //                 {
        //                     ModelState.AddModelError(string.Empty, "競合が検出されました。");
        //                     return View(book);
        //                 }
        //             }
        //             return RedirectToAction(nameof(Index));
        //         }
        //         return View(book);
        //     }
        //     return View(book);
        // }

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
    }
