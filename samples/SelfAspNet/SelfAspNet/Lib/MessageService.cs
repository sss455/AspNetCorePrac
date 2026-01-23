namespace SelfAspNet.Lib;

public interface IMessageService
{
  string Message { get; }

  string? ToString() => Message;
}

public class MorningMessageService : IMessageService
{
  public string Message
  {
    get => "Good Morning!";
  }
}

public class NightMessageService : IMessageService
{
  public string Message
  {
    get => "Good Night!";
  }
}