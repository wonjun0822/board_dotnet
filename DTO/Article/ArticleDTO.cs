namespace board_dotnet.DTO
{
    /// <summary>
    /// 게시글 목록 정보
    /// </summary>
    public record ArticleDTO {
        /// <summary>
        /// 게시글 ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        public string title { get; set; } = string.Empty;

        /// <summary>
        /// 조회 수
        /// </summary>
        public int viewCount { get; set; } = 0;

        /// <summary>
        /// 작성자 닉네임
        /// </summary>
        public string nickname { get; set; } = string.Empty;

        /// <summary>
        /// 최종 수정일
        /// </summary>
        public DateTime updateAt { get; set; }
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