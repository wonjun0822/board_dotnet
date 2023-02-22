namespace board_dotnet.DTO;

public record CommentResultDTO {
    public long articleId { get; set; }
    public long commentId { get; set; }
    public string comment { get; set; } = string.Empty;
    public string nickname { get; set; } = string.Empty;
    public DateTime createAt { get; set; }
    public DateTime updateAt { get; set; }
}