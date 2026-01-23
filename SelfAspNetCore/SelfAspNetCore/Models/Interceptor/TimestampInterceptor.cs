using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mono.TextTemplating;

namespace SelfAspNetCore.Models.Interceptor;

// p.301 [Add] エンティティを操作した前後の処理を実装する（インタセプタ―）
//
// インタセプタ―であることのルールは、IInterceptorインターフェイスを実装していること。
// ただし、実際のアプリからIInterceptorを直接実装する機会はほとんどなく、これを実装した中小クラスを継承するのが一般的。
// インタセプタ―定義のための基底クラスは、p.303の表5.20を参照。
public class TimestampInterceptor : SaveChangesInterceptor // IInterceptorを継承した、SaveChangesInterceptor抽象クラスを継承
{
    // SaveChangesInterceptorクラスの主な仮想メソッドは、p.304の表5.21を参照。
    // この例ではエンティティを保存する前に、作成／更新日時を更新するため、SavingChanges／SavingChangesAsyncメソッドを双方オーバーライドする

    // SavingChangesメソッドをオーバーライド
    // SavingChangesメソッド：SaveChangesメソッドの実行前に呼び出し
    public override InterceptionResult<int> SavingChanges(
                                                DbContextEventData eventData, 
                                                InterceptionResult<int> result)
    {
        // 現在のコンテキストに基づいてタイムスタンプを更新（自前の更新処理を間に挟む）
        UpdateTimestamp(eventData.Context!);

        // 基底クラスのSavingChangesメソッドを呼び出し
        return base.SavingChanges(eventData, result);
        
    }

    // SavingChangesAsyncメソッドをオーバーライド
    // SavingChangesAsyncメソッド：SaveChangesAsyncメソッドの実行前に呼び出し
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
                                                            DbContextEventData eventData,
                                                            InterceptionResult<int> result,
                                                            CancellationToken cancellationToken = default)
    {
        // 現在のコンテキストに基づいてタイムスタンプを更新（自前の更新処理を間に挟む）
        UpdateTimestamp(eventData.Context!);

        // 基底クラスのSavingChangesAsyncメソッドを呼び出し
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    // 作成日／更新日時に現在日時を設定
    private static void UpdateTimestamp(DbContext db)
    {
        // 現在日時
        var current = DateTime.Now;

        // コンテキストが現在追跡しているエンティティは、DbContextオブジェクトからChangeTracker.Entriesメソッドで取得できる
        // var = EntityEntry
        foreach(var e in db.ChangeTracker.Entries())
        {
            // タイムスタンプを記録可能なエンティティの場合（＝IRecordableTimestampインターフェイスを実装している）
            if( e.Entity is IRecordableTimestamp te)
            {
                // エンティティの状態を表すのは「EntityEntry#Stateプロパティ」
                switch (e.State)
                {
                    // エンティティの状態が「作成」の場合
                    case EntityState.Added:
                        // 作成／更新日時に現在日時を設定
                        te.CreatedAt = current;
                        te.UpdatedAt = current;
                        break;
                    // エンティティの状態が「更新」の場合
                    case EntityState.Modified:
                        // 更新日時だけに現在日時を設定
                        te.UpdatedAt = current;
                        break;
                }
            }
        }
    }
}
