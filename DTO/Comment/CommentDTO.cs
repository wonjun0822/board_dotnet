namespace board_dotnet.DTO
{
    //[SwaggerExclude]
    public record CommentDTO {
        public long articleId { get; set; }
        public long commentId { get; set; }
        public string comment { get; set; } = string.Empty;
        public string nickname { get; set; } = string.Empty;
        public DateTime updateAt { get; set; }
        public bool isModify { get; set; } = false;
    } 
}