using Microsoft.EntityFrameworkCore;

namespace CoreEntity.Models;

[Keyless]
public class ViewPubCount
{
  public string Publisher { get; set; } = String.Empty;
  public int BookCount { get; set; }
}