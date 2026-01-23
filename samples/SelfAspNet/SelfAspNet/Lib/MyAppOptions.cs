using System.ComponentModel.DataAnnotations;

namespace SelfAspNet.Lib;
public class MyAppOptions
{
    // [MaxLength(10)]
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime Published { get; set; }
    public List<string> Projects { get; set; } = new();
}