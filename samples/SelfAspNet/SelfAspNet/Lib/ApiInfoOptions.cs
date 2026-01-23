namespace SelfAspNet.Lib;

public class ApiInfoOptions
{
  public const string SlideShow = nameof(SlideShow);
  public const string OpenWeather = nameof(OpenWeather);

  public string BaseUrl { get; set; } = string.Empty;
  public string Key { get; set; } = string.Empty;
  public string? Secret { get; set; }
}