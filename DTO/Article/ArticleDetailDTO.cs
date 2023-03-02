namespace board_dotnet.DTO
{
    /// <summary>
    /// 게시글 상세정보
    /// </summary>
    public record ArticleDetailDTO {
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
        /// 조회 수
        /// </summary>
        public int viewCount { get; set; } = 0;

        /// <summary>
        /// 작성자 닉네임
        /// </summary>
        public string nickname { get; set; } = string.Empty;

        /// <summary>
        /// 최종 수정일
        /// </summary>
        public DateTime updateAt { get; set; }

        /// <summary>
        /// 수정 가능 여부
        /// </summary>
        public bool isModify { get; set; } = false;

        /// <summary>
        /// 게시글 댓글 목록
        /// </summary>
        public virtual ICollection<CommentDTO> comments { get; set; } = new HashSet<CommentDTO>();

        /// <summary>
        /// 게시글 첨부파일 목록
        /// </summary>
        public virtual ICollection<AttachFileDTO> files { get; set; } = new HashSet<AttachFileDTO>();
    }
}