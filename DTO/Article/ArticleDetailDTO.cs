namespace board_dotnet.DTO
{
    public record ArticleDetailDTO {
        public long id { get; set; }
        public string title { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public int viewCount { get; set; } = 0;
        public string nickname { get; set; } = string.Empty;
        public DateTime updateAt { get; set; }

        public virtual ICollection<CommentDTO> comments { get; set; } = new HashSet<CommentDTO>();
        public virtual ICollection<AttachFileDTO> files { get; set; } = new HashSet<AttachFileDTO>();
    }
}