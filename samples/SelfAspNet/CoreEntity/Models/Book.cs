using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreEntity.Models;

[Table("Contents")]
public class Book
{
    public int Id { get; set; }

    [Column(Order = 0, TypeName = "CHAR(17)")]
    public string Isbn { get; set; } = String.Empty;

    public string Title { get; set; } = String.Empty;

    [Column("Amount", Order=1, TypeName="NVARCHAR(50)")]
    public int Price { get; set; }

    // [Precision(7, 2)]
    // public decimal Price { get; set; }

    // [MaxLength(30)]
    public string Publisher { get; set; } = String.Empty;

    // [Precision(5)]
    public DateTime Published { get; set; }

    public bool Sample { get; set; }

    public ICollection<Review> Reviews { get; } = new List<Review>();
    public ICollection<Author> Authors { get; } = new List<Author>();
}
