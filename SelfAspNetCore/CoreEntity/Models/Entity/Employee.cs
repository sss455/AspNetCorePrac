using System;

namespace CoreEntity.Models;

//---------------------------------------------------
// 従業員情報エンティティ（複合型の利用側）
//---------------------------------------------------
// p.229 [Add] ComplexType属性：複数の値を複合型として切り出す
public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // 複合型として切り出した住所情報エンティティを参照
    public Address Address { get; set; } = null!;
}
