using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreEntity.Models;

// p.231 [Add] 継承関係にあるエンティティのマッピング

//---------------------------------------------------
// 記事エンティティ
//---------------------------------------------------
public class Article
{
    public int Id { get; set; }
    public string Url   { get; set; } = string.Empty;  // 記事の掲載先URL
    public string Title { get; set; } = string.Empty;  // 記事タイトル
}

//---------------------------------------------------
// タイアップ記事エンティティ（記事エンティティを継承）
//---------------------------------------------------
public class CollabArticle : Article
{
    public string Company { get; set; } = string.Empty;  // 提携先会社
}

public enum Articles {
    Article,
    CollabArticle
};