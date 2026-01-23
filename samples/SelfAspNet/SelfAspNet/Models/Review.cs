using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SelfAspNet.Models;

public class Review
{
    public int Id { get; set; }

    [Display(Name = "名前")]
    public string Name { get; set; } = String.Empty;

    [CustomValidation(typeof(Review), nameof(CheckNgword))]
    [Display(Name = "レビュー")]
    public string Body { get; set; } = String.Empty;

    [Display(Name = "更新日")]
    public DateTime LastUpdated { get; set; } = DateTime.Now;

    [Display(Name = "書籍")]
    public int BookId { get; set; }
    // [DeleteBehavior(DeleteBehavior.SetNull)]
    public Book Book { get; set; } = null!;

    // public virtual int BookId { get; set; }
    // public virtual Book Book { get; set; } = null!;

    public static ValidationResult CheckNgword(
        string body, ValidationContext context)
    {

        string[] ngList = ["中毒", "詐欺", "薬物"];
        foreach (var data in ngList)
        {
            if (body.Contains(data))
            {
                return new ValidationResult("本文内で禁止用語が使われています。");
            }
        }
        return ValidationResult.Success!;
    }
}
