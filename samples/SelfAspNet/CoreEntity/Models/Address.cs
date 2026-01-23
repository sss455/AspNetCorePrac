using System.ComponentModel.DataAnnotations.Schema;

namespace CoreEntity.Models;

[ComplexType]
public class Address
{
  public string PostNumber { get; set; } = string.Empty;
  public string Prefecture { get; set; } = string.Empty;
  public string City { get; set; } = string.Empty;
  public string Other { get; set; } = string.Empty;
}
