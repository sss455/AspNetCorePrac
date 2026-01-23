namespace SelfAspNet.Lib;

public interface IHogeService
{
  string Value { get; }
}

public class HogeService : IHogeService
{
  private readonly string? _hoge;
  public HogeService(string? hoge)
  {
    _hoge = hoge;
  }

  public string Value => _hoge!;
}