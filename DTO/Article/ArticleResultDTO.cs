namespace board_dotnet.DTO;

/// <summary>
/// 게시글 저장 또는 수정 결과
/// </summary>
public record ArticleResultDTO {
    /// <summary>
    /// 게시글 ID
    /// </summary>
    public long id { get; set; }
    
    /// <summary>
    /// 제목
    /// </summary>
    public string title { get; set; } = string.Empty;

    /// <summary>
    /// 내용
    /// </summary>
    public string content { get; set; } = string.Empty;

    /// <summary>
    /// 작성일
    /// </summary>
    public DateTime createAt { get; set; }

    /// <summary>
    /// 수정일
    /// </summary>
    public DateTime updateAt { get; set; }
}