namespace board_dotnet.DTO
{
    //[SwaggerExclude]
    public record CommentDTO {
        public long articleId { get; set; }
        public long commentId { get; set; }
        public string comment { get; set; } = string.Empty;
        public string nickname { get; set; } = string.Empty;
        public DateTime createAt { get; set; }
    } 
}