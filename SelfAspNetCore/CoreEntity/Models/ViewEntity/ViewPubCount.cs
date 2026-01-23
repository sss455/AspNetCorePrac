using System;
using Microsoft.EntityFrameworkCore;

namespace CoreEntity.Models.ViewEntity;

// p.251 [Add] マイグレーションでビューを定義する（Sqlメソッド）
// キーなしエンティティ
[Keyless]
public class ViewPubCount // ビューエンティティ
{
    // 出版社
    public string Publisher { get; set; } = string.Empty;

    // 出版社ごとの書籍の件数
    public int BookCount { get; set; }
}
