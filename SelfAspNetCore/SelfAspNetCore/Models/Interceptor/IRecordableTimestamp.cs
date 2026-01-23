using System;

namespace SelfAspNetCore.Models.Interceptor;

// p.301 [Add] エンティティを操作した前後の処理を実装する（インタセプタ―）

// タイムスタンプを記録できる型として、IRecordableTimestampインターフェイスを準備。
// ※このインターフェイスを継承したエンティティ(テーブル)だけを、作成／更新日時列の更新処理の対象にしたいため。
public interface IRecordableTimestamp
{
    // 作成日時
    public DateTime CreatedAt { get; set; }
    // 更新日時
    public DateTime UpdatedAt { get; set; }
}
