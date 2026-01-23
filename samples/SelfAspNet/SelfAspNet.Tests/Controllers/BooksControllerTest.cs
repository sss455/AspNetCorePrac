using Microsoft.AspNetCore.Mvc;
using SelfAspNet.Controllers;
using SelfAspNet.Models;
using SelfAspNet.Tests.Models;

namespace SelfAspNet.Tests.Controllers;

[TestClass]
public class BooksControllerTest
{
    // 以下のTestIndexテストは、リポジトリクラスの利用を前提としています。
    // BooksControllerから以下のコードを有効にしたうえで、テストを実行してください。
    //
    // private readonly IBookRepository _rep;
    // public BooksController(IBookRepository rep, MyContext context)
    // {
    //     _rep = rep;
    //     _context = context;
    // }
    // ...中略...
    // public async Task<IActionResult> Index()
    // {
    //     return View(await _rep.GetAllAsync());
    // }

    // [TestMethod]
    // public async Task TestIndex()
    // {
    //     var controller = new BooksController(new BookRepositoryMock(), null!);
    //     var result = await controller.Index() as ViewResult;
    //     Assert.IsNotNull(result?.Model);
    //     Assert.AreEqual(2, (result?.Model as List<Book>)?.Count);
    // }
}