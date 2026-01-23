using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chapter07.Models;

// レビュー情報テーブルエンティティ
public class Review
{
    // レビューID
    public int Id { get; set; }

    [Display(Name = "氏名")]
    public string Name { get; set; } = String.Empty;

    // p.315 [Add] 独自の検証ルールを実装する(プロパティ)ーー CustomValidation属性
    //  ・第1引数：検証メソッドが属する型（今回は自分自身）
    //  ・第2引数：検証メソッドの名前
    // ※CustomValidation属性による検証は、サーバーサイドでしか動作しない）
    [CustomValidation(typeof(Review), nameof(CheckNgword))]
    [Display(Name = "レビュー")]
    public string Body { get; set; } = String.Empty;

    [Display(Name = "更新日時")]
    public DateTime LastUpdated { get; set; } = DateTime.Now;

    // 外部キーを表すプロパティ
    // ※外部キーは「ナビゲーションプロパティ＋Id」のように表す。
    [Display(Name = "書籍ID")]
    public int BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    public Book Book { get; set; } = null!;
    //-------------------------------------------------------------------------------------
    //【補足】
    //   Review→Bookの参照のような関係を表すナビゲーションプロパティは、以下のようなルールで構成する。
    //    ・名前は単数形（ここではBook）
    //    ・戻り値はエンティティ型（ここではBook型）
    //    ・一般的には、ゲッター／セッターともに設置する（セッターはpublicでなくてもよい）
    //-------------------------------------------------------------------------------------



    // // p.219 [Add]「ナビゲーションプロパティへのアクセス(遅延読み込み)」用にvirtual修飾子を付与。
    // public virtual int BookId { get; set; }
    // public virtual Book Book { get; set; } = null!;


    // p.291 [Add] エンティティの関係を削除する
    // ※参照先の書籍情報を削除したときも、紐づいたレビュー情報は残したい場合、
    //  「参照ナビゲーション＋外部キープロパティ」をNull許容型に変更する。
    // public int? BookId { get; set; }  // 外部キープロパティ
    // public Book? Book { get; set; }   // 参照ナビゲーション



    // p.315 [Add] 独自の検証ルールを実装する(プロパティ)ーー CustomValidation属性
    // カスタム検証ルールの本体
    public static ValidationResult CheckNgword(
                //ValidationResult：検証結果（ValidationResultオブジェクト）
                                        string body,                // Bodyプロパティの値(レビュー本文)が渡される
                                        ValidationContext context)  // 検証コンテキスト(今回は利用していない) cf.p.316 表5.25
    {
        // 禁止用語リスト
        string[] ngList = [ "中毒", "詐欺", "薬物" ];
        //var ngList = new List<string> { "中毒", "詐欺", "薬物" };

        foreach(var ngword in ngList)
        {
            // レビュー本文に禁止用語が含まれていれば検証エラー
            if(body.Contains(ngword))
            {
                return new ValidationResult("本文内で禁止用語が使われています。");
                //----------------------------------------------------------------
                //【構文】ValidationResultコンストラクター
                //   ValidationResult(string? errorMessage [, IEnumerable<string>? memberNames])
                //    ・errorMessage：エラーメッセージ
                //    ・memberNames ：検証エラーのあるメンバー名のリスト
                //----------------------------------------------------------------
            }
        }
        // すべての禁止用語がなければ検証成功
        return ValidationResult.Success!;
    }

}
