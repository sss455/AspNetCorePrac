using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfAspNet.Models;

namespace CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [FormatFilter]
    public class BooksController : ControllerBase
    {
        private readonly MyContext _context;

        public BooksController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();

            // return await _context.Books
            // .Include(b => b.Authors)
            // .Include(b => b.Reviews)
            // .ToListAsync();
        }

        // GET: api/Books/5
        // [FormatFilter]
        [HttpGet("{id}")]
        // [HttpGet("{id}.{format?}")]
        // [Produces("application/xml")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // 戻り値型をTask<IActionResult>型とした場合
        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetBook(int id)
        // {
        //     var book = await _context.Books.FindAsync(id);

        //     if (book == null)
        //     {
        //         return NotFound();
        //     }

        //     return new ObjectResult(book);
        // }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        /// <summary>
        /// 既存の書籍情報を更新する
        /// </summary>
        /// <param name="id">書籍コード</param>
        /// <remarks>
        /// リクエスト情報（例）：
        ///
        ///     PUT /api/book/1
        ///     {
        ///       "isbn": "978-4-7981-9999-9",
        ///       "title": "速習 ASP.NET Core",
        ///       "price": 3000,
        ///       "publisher": "WINGS",
        ///       "published": "2024-02-07",
        ///       "sample": true
        ///     }
        ///
        /// </remarks>
        /// <response code="204">正しく書籍情報が更新された</response>
        /// <response code="400">引数idと更新対象の書籍コードが一致しない</response>
        /// <response code="404">更新時に対象の書籍が削除されていた</response>
        [HttpPut("{id}")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [Consumes("application/xml")]
        // [SkipStatusCodePages]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // IResult型での書き換え
        // [HttpPut("{id}")]
        // public async Task<IResult> PutBook(int id, Book book)
        // {
        //     if (id != book.Id)
        //     {
        //         Results.BadRequest();
        //     }

        //     _context.Entry(book).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!BookExists(id))
        //         {
        //             Results.NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return Results.NoContent();
        // }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
