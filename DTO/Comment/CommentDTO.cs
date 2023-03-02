namespace board_dotnet.DTO
{
    //[SwaggerExclude]
    public record CommentDTO {
        /// <summary>
        /// 게시글 ID
        /// </summary>
        public long articleId { get; set; }

        /// <summary>
        /// 게시글 댓글 ID
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
        /// 댓글 최종 수정일
        /// </summary>
        public DateTime updateAt { get; set; }

        /// <summary>
        /// 댓글 수정 가능 여부
        /// </summary>
        public bool isModify { get; set; } = false;
    } 
}