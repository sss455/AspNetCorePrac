using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreEntity.Models.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;

namespace CoreEntity.Models;

//---------------------------------------------
// レビュー情報テーブルエンティティ
//---------------------------------------------

// p.236 [Add]【別解】エンティティ単位(Review)に分割したモデル定義を、対象エンティティに対して直接紐づけることも可能
//[EntityTypeConfiguration(typeof(ReviewEntityTypeConfiguration))]
public class Review
{
    // // レビューID
    // public int Id { get; set; }

    // p.226 [Add] Key属性：命名ルールとは異なる列に主キーを指定
    [Key]
    public int Code { get; set; }

    public string Name { get; set; } = String.Empty;

    public string Body { get; set; } = String.Empty;

    public DateTime LastUpdated { get; set; } = DateTime.Now;

    // // 外部キーを表すプロパティ
    // // ※外部キーは「ナビゲーションプロパティ＋Id」のように表す。
    // public int BookId { get; set; }

    // 外部キー
    public int ForBook { get; set; }

    // p.226 [Add] ForeignKeyy属性：命名ルールとは異なる列に外部キーを指定
    [ForeignKey(nameof(ForBook))]
    public Book Book { get; set; } = null!;



    // p.227 [Add] NotMapped属性：特定のプロパティを列マッピングから除外する
    //  ※既存の列を加工／演算した結果を返すプロパティをマッピングから除外
    [NotMapped]
    public string Summary
    {
        // ゲッター
        get
        {
            // 30文字以内であれば、元のBody値を返す
            if(Body.Length < 30) return Body;

            // 本文(Bodyプロパティ)の先頭30文字を抜き出して返す
            return Body[..30] + "...";
        }
    }

}
