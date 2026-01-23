using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.IdentityModel.Tokens;

namespace SelfAspNetCore.Models;

// ユーザー情報テーブルエンティティ

// p.318 [Add] 独自の検証ルールを実装する(モデル)ーー CustomValidation属性
//  ・第1引数：検証メソッドが属する型（今回は自分自身）
//  ・第2引数：検証メソッドの名前
// ※モデルレベルの場合、クラスに対してCustomValidation属性を付与する
// ※CustomValidation属性による検証は、サーバーサイドでしか動作しない）
[CustomValidation(typeof(User), nameof(ValidateEmailForNews))]

// p.319 [Mod] 独自の検証ルールを実装する(3)ーー IValidatableObjectインターフェイス
// ※IValidatableObjectインターフェイスを実装することでも、エンティティレベルの検証ルールを設置できる
//public class User
public class User : IValidatableObject // IValidatableObjectインターフェイスを実装
{
    // ユーザーID
    public int Id { get; set; }

    [Display(Name = "氏名")]
    public string Name { get; set; } = String.Empty;

    [EmailAddress(ErrorMessage="{0}はメールアドレスの形式で入力してください。")]
    [Display(Name = "メールアドレス")]
    //public string Email { get; set; } = String.Empty;
    public string? Email { get; set; }

    // p.314 [Add] Compare検証：2個のプロパティが互いに等しいかを判定
    // 判定用に「メールアドレス(確認)」項目を追加。
    // ※あくまでCompare検証のための便宜的なプロパティで、データベースに反映しないため、
    //   NotMapped属性を付与してデータベースとのマッピングを解除する。
    [NotMapped]
    [Compare(nameof(Email), ErrorMessage="{0}が、{1}と一致していません。")]
    [Display(Name = "メールアドレス（確認）")]
    public string? EmailConfirmed { get; set; }


    [Display(Name = "誕生日")]
    public DateTime Birth { get; set; }

    [Display(Name = "メールニュースの希望")]
    public bool NeedNews { get; set; }

    // ナビゲーションプロパティ（リレーションシップ）
    public Author? Author { get; set; }

    
    // // p.219 [Add]「ナビゲーションプロパティへのアクセス(遅延読み込み)」用にvirtual修飾子を付与。
    // // ナビゲーションプロパティ（リレーションシップ）
    // public virtual Author? Author { get; set; }




    // p.318 [Add] 独自の検証ルールを実装する(モデル)ーー CustomValidation属性
    // ※モデル(エンティティ)レベルの場合、主に複数のプロパティにまたがるような値検証を実施したい場合に利用する。
    // カスタム検証ルールの本体
    public static ValidationResult ValidateEmailForNews(User user) // 第1引数はエンティティを受け取る
    {
        //「メールニュースの要否」がtrueである場合にだけ、メールアドレスを必須にする
        if(user.NeedNews && string.IsNullOrEmpty(user.Email))
        {
            return new ValidationResult(
                    "メールニュースを受け取るにはメールアドレスは必須です。"
                    // 明示的にEmail列にエラーを紐づけることも可能
                    //, new [] { nameof(Email) }
                );
            //----------------------------------------------------------------
            //【構文】ValidationResultコンストラクター
            //   ValidationResult(string? errorMessage [, IEnumerable<string>? memberNames])
            //    ・errorMessage：エラーメッセージ
            //    ・memberNames ：検証エラーのあるメンバー名のリスト
            //----------------------------------------------------------------

        }
        // 検証成功
        return ValidationResult.Success!;
    }

    // p.319 [Mod] 独自の検証ルールを実装する(3)ーー IValidatableObjectインターフェイス
    // 検証ルールは、IValidatableObject#Validateメソッドに実装
    // ※戻り値がIEnumerable<ValidationResult>型（エラー情報のリスト）なので、戻り値もyield return命令で返す。
    //  （イテレーター構文）
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        //「メールニュースの要否」がtrueである場合にだけ、メールアドレスを必須にする
        if(NeedNews && string.IsNullOrEmpty(Email))
        {
            // yield return命令で返す(イテレーター構文)
            yield return new ValidationResult(
                    "メールニュースを受け取るにはメールアドレスは必須です。"
                    // 明示的にEmail列にエラーを紐づけることも可能
                    //, new [] { nameof(Email) }
                );
            //----------------------------------------------------------------
            //【構文】ValidationResultコンストラクター
            //   ValidationResult(string? errorMessage [, IEnumerable<string>? memberNames])
            //    ・errorMessage：エラーメッセージ
            //    ・memberNames ：検証エラーのあるメンバー名のリスト
            //----------------------------------------------------------------

        }
        // 成功時には何も返さない。
    }
}
