namespace SelfAspNet.Lib;

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(
      this ILoggingBuilder builder, string filePath)
    {
        builder.AddProvider(new FileLogProvider(filePath));
        return builder;
    }
}