using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreEntity.Models;

[Index(nameof(LastName), nameof(FirstName), Name = "Index_FullName")]
// [Index(nameof(LastName), nameof(FirstName), AllDescending=false)]
// [Index(nameof(LastName), nameof(FirstName), IsDescending = new[] { false, true })]
public class User
{
    public int Id { get; set; }

    public string LastName { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;

    public EmailAddress? Email { get; set; }

    public DateTime Birth { get; set; } = DateTime.Now;
    // [Column(TypeName = "nvarchar(15)")]
    public UserClass UserClass { get; set; } = UserClass.Guest;

    public Author Author { get; set; } = null!;
}