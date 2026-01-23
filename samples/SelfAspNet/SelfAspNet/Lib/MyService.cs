namespace SelfAspNet.Lib;

public interface IMyService
{
  Guid Id { get; }
}

public class MyService : IMyService
{
  private readonly Guid _id;
  public MyService()
  {
    _id = Guid.NewGuid();
  }

  public Guid Id => _id;
}