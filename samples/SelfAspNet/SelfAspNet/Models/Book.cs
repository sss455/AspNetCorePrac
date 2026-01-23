using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SelfAspNet.Lib;

namespace SelfAspNet.Models;
public class Book
{
    public int Id { get; set; }
    
    // [RegularExpression("978-4-[0-9]{2,5}-[0-9]{2,5}-[0-9X]",
    //     ErrorMessage = "{0}の形式が誤っています。")]
    // [Required(ErrorMessage = "RequiredError")]
    // [UIHint("Isbn")]
    // [Remote("UniqueIsbn", "Books")]
    // [Remote("UniqueIsbn", "Books", AdditionalFields=nameof(Title))]
    // [DataType(DataType.ImageUrl)]
    [Display(Name = "ISBN")]
    // [Display(Name = "Book_Isbn")]
    public string Isbn { get; set; } = String.Empty;

    // [Required(ErrorMessage = "{0}は必須です。")]
    // [StringLength(50, ErrorMessage = "{0}は{1}文字以内で指定してください。")]
    // [Required(ErrorMessage = "RequiredError")]
    [Display(Name = "タイトル")]
    // [Display(Name = "Book_Title")]
    public string Title { get; set; } = String.Empty;

    // [Range(10, 10000, ErrorMessage = "{0}は{1}～{2}の間で指定してください。")]
    // [Range(10, 10000, ErrorMessage = "RangeError")]
    // [DataType(DataType.Currency)]
    [Display(Name = "価格")]
    // [Display(Name = "Book_Price")]
    public int Price { get; set; }

    // [RegularExpression("翔泳社|技術評論社|SBクリエイティブ|日経BP|森北出版",
    // ErrorMessage = "{0}は「{1}」のいずれかでなければなりません。")]
    // [Required(ErrorMessage = "RequiredError")]
    // [InOptions("翔泳社,技術評論社,SBクリエイティブ,日経BP,森北出版")]
    [Display(Name = "出版社")]
    // [Display(Name = "Book_Publisher")]
    public string Publisher { get; set; } = String.Empty;

    // [Range(typeof(DateTime), "2010-01-01", "2029-12-31",
    //     ErrorMessage = "{0}は{1:d}～{2:d}の範囲で指定してください。")]
    // [Required(ErrorMessage = "RequiredError")]
    // [BindNever]
    // [DataType(DataType.Date)]
    [Display(Name = "刊行日")]
    // [Display(Name = "Book_Published")]
    public DateTime Published { get; set; }

    [Display(Name = "配布サンプル")]
    // [Display(Name = "Book_Sample")]
    public bool Sample { get; set; }

    // [Timestamp]
    // public byte[]? RowVersion { get; set; }

    public ICollection<Review> Reviews { get; } = new List<Review>();
    public ICollection<Author> Authors { get; } = new List<Author>();

    // public virtual ICollection<Review> Reviews { get; } = new List<Review>();
    // public virtual ICollection<Author> Authors { get; } = new List<Author>();
}
