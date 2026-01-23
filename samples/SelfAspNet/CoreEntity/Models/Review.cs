using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CoreEntity.Models;

// [PrimaryKey(nameof(Code), nameof(Name))]
// [EntityTypeConfiguration(typeof(ReviewEntityTypeConfiguration))]
public class Review
{
    [Key]
    public int Code { get; set; }

    public string Name { get; set; } = String.Empty;

    public string Body { get; set; } = String.Empty;

    public DateTime LastUpdated { get; set; } = DateTime.Now;

    public int ForBook { get; set; }
    [ForeignKey(nameof(ForBook))]
    public Book Book { get; set; } = null!;

    // [NotMapped]
    // public string Summary
    // {
    //     get
    //     {
    //         if (Body.Length < 30) { return Body; }
    //         return Body[..30] + "...";
    //     }
    // }
}
