namespace board_dotnet.DTO
{
    //[SwaggerExclude]
    public record ArticleDTO {
        public long id { get; set; }
        public string title { get; set; } = string.Empty;
        public int viewCount { get; set; } = 0;
        public string nickname { get; set; } = string.Empty;
        public DateTime createAt { get; set; }
    }

    // public static explicit operator Articles(Article article)
    // {
    //     var response = new Articles
    //     {
    //         id = article.id,
    //         title = article.title,
    //         viewCount = article.viewCount
    //     };

    //     return response;
    // }
}