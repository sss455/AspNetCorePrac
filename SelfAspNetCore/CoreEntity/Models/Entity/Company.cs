using System;

namespace CoreEntity.Models;

//---------------------------------------------------
// 会社情報エンティティ（複合型の利用側）
//---------------------------------------------------
// p.229 [Add] ComplexType属性：複数の値を複合型として切り出す
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // 複合型として切り出した住所情報エンティティを参照
    public Address Address { get; set; } = null!;
}
