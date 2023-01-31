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

    public record ArticleDetailDTO {
        public long id { get; set; }
        public string title { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public int viewCount { get; set; } = 0;
        public string nickname { get; set; } = string.Empty;
        public DateTime createAt { get; set; }

        public virtual ICollection<CommentDTO> comments { get; set; } = new HashSet<CommentDTO>();
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