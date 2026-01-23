using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SelfAspNet.Models;

namespace CoreRazor.Pages_Books
{
    public class IndexModel : PageModel
    {
        private readonly SelfAspNet.Models.MyContext _context;

        public IndexModel(SelfAspNet.Models.MyContext context)
        {
            _context = context;
        }

        public IList<Book> Book { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Book = await _context.Books.ToListAsync();
        }

        // public async Task<IActionResult> OnGetAsync()
        // {
        //     Book = await _context.Books.ToListAsync();
        //     return Page();
        // }
    }
}
