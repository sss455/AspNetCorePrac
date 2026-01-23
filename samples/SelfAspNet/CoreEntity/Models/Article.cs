namespace CoreEntity.Models;

public class Article
{
    public int Id { get; set; }
    public string Url { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
}

public class CollabArticle : Article
{
    public string Company { get; set; } = String.Empty;
}