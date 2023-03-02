using System.ComponentModel.DataAnnotations;

namespace board_dotnet.DTO;
 
 public record CommentWriteDTO {
    /// <summary>
    /// 댓글 내용
    /// </summary>
    [Required]
    public string comment { get; set; } = string.Empty;
}