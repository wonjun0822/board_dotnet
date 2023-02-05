using System.ComponentModel.DataAnnotations;

namespace board_dotnet.DTO;
 
 public record CommentWriteDTO {
    [Required]
    public string comment { get; set; } = string.Empty;
}