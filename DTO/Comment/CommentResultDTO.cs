namespace board_dotnet.DTO;

public record CommentResultDTO {
    /// <summary>
    /// 게시글 ID
    /// </summary>
    public long articleId { get; set; }

    /// <summary>
    /// 댓글 ID
    /// </summary>
    public long commentId { get; set; }

    /// <summary>
    /// 댓글 내용
    /// </summary>
    public string comment { get; set; } = string.Empty;

    /// <summary>
    /// 댓글 작성자 닉네임
    /// </summary>
    public string nickname { get; set; } = string.Empty;

    /// <summary>
    /// 댓글 작성일
    /// </summary>
    public DateTime createAt { get; set; }

    /// <summary>
    /// 댓글 최종 수정일
    /// </summary>
    public DateTime updateAt { get; set; }
}