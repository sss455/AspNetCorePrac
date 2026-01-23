using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using SelfAspNet.Models;

namespace SelfAspNet.Lib;

public class ListViewComponent : ViewComponent
{
    private readonly MyContext _db;

    public ListViewComponent(MyContext db)
    {
        _db = db;
    }

    public async Task<IViewComponentResult> InvokeAsync(int price)
    {
        return View(await _db.Books
            .Where(b => b.Price < price).ToListAsync());
    }

    // public async Task<IViewComponentResult> InvokeAsync(int highPrice)
    // {
    //     return View(await _db.Books
    //         .Where(b => b.Price < highPrice).ToListAsync());
    // }
}
