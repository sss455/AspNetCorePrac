using CoreEntity.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreEntity.Lib;
public class EmailAddressConverter : ValueConverter<EmailAddress, string>
{
  public EmailAddressConverter()
    : base(
      v => v.ToString(),
      v => new EmailAddress(v)
    ) { }
}