namespace SelfAspNet.Models;

public class ErrorLog
{
    public int Id { get; set; }
    public string Path { get; set; } = String.Empty;
    public string Message { get; set; } = String.Empty;
    public string Stacktrace { get; set; } = String.Empty;
    public DateTime Accessed { get; set; } = DateTime.Now;
}
