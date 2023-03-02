using System.ComponentModel.DataAnnotations;

namespace board_dotnet.DTO
{
    public record ArticleWriteDTO 
    {
        [Required]
        public string title { get; set; } = string.Empty;

        [Required]
        public string content { get; set; } = string.Empty;

        public IFormFileCollection? files { get; set; }
    }
}