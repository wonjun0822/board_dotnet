namespace board_dotnet.DTO;

public record ArticleResultDTO {
    public long id { get; set; }
    public string title { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;
    public string nickname { get; set; } = string.Empty;
    public DateTime createAt { get; set; }
    public DateTime updateAt { get; set; }
}