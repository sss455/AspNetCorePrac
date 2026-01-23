using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreEntity.Models;

//---------------------------------------------------
// 住所情報エンティティ（住所情報を複合型として切り出し）
//---------------------------------------------------
// p.229 [Add] ComplexType属性：複数の値を複合型として切り出す
// 複合型：特定のプロパティ群を切り出した型のこと
[ComplexType]
public class Address
{
    //--------------------------------------------------------------------
    // Companyエンティティ、Employeeエンティティの共通プロパティとして利用する
    //--------------------------------------------------------------------

    // ※複合型は、主キーを持たない

    public string PostNumber { get; set; } = string.Empty;  // 郵便番号
    public string Prefecture { get; set; } = string.Empty;  // 都道府県
    public string City       { get; set; } = string.Empty;  // 市町村
    public string Other      { get; set; } = string.Empty;  // 番地・建物など
}
