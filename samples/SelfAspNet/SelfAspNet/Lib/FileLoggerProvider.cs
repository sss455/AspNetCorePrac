namespace SelfAspNet.Lib;

public class FileLogProvider : ILoggerProvider
{
    private readonly string _filePath;

    public FileLogProvider(string filePath)
    {
        _filePath = filePath;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(_filePath, categoryName);
    }

    public void Dispose() { }
}
