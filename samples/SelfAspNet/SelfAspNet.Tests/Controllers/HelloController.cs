using Microsoft.AspNetCore.Mvc;
using SelfAspNet.Controllers;

namespace SelfAspNet.Tests.Controllers;

[TestClass]
public class HelloControllerTest
{
    [TestMethod]
    public void TestShow()
    {
        var controller = new HelloController(null!);
        var result = controller.Show();
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.AreEqual("こんにちは、世界！", controller.ViewBag.Message);
    }
}