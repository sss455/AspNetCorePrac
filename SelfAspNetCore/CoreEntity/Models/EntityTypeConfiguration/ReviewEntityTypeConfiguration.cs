using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreEntity.Models.EntityTypeConfiguration;

// p.235 [Add] モデル定義をエンティティ単位に分割する（Fluent API）
public class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Comments")         // テーブルを設定（ReviewエンティティをCommentsテーブルに紐づけ）
                .HasKey(r => r.Code);       // 主キーを設定 （Codeプロパティ）

        builder.Property(r => r.Body)       // プロパティを取得（Bodyプロパティ）
                .HasColumnName("Message")   // 列名を設定（Bodyプロパティに対応する列名をMessageに）
                .HasMaxLength(150);         // 最大長を設定（Bodyプロパティの最大長を150文字に）
    }
}

// この後、MyContext.csから「ReviewEntityTypeConfiguration#Configureメソッド」を呼び出す。
// または、別解としてReviewエンティティに対して「IEntityTypeConfiguration<TEntity>実装クラス」を直接紐づける。