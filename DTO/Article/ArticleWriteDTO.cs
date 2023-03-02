using System.ComponentModel.DataAnnotations;

namespace board_dotnet.DTO
{
    /// <summary>
    /// 게시글 작성 정보
    /// </summary>
    public record ArticleWriteDTO 
    {
        /// <summary>
        /// 제목
        /// </summary>
        [Required]
        public string title { get; set; } = string.Empty;

        /// <summary>
        /// 내용
        /// </summary>
        [Required]
        public string content { get; set; } = string.Empty;

        /// <summary>
        /// 첨부파일
        /// </summary>
        public IFormFileCollection? files { get; set; }
    }
}