// p.453 [Add] AddSingleton／AddScoped／AddTransientメソッドのオーバーロード
namespace Chapter07.Lib;

public interface IMessageService
{
    string Message { get; }

    string? ToString() => Message;
}


// 朝のメッセージを生成
public class MorningMessageService : IMessageService
{
    public string Message
    {
        get => "Good Morning!"; 
    }
}

// 夜のメッセージを生成
public class NightMessageService : IMessageService
{
    public string Message
    {
        get => "Good Night!"; 
    }
}
