namespace board_dotnet.DTO
{
    //[SwaggerExclude]
    public record AttachFileDTO {
        /// <summary>
        /// 게시글 ID
        /// </summary>
        public long articleId { get; set; }

        /// <summary>
        /// 게시글 첨부파일 ID
        /// </summary>
        public long attachFileId { get; set; }

        /// <summary>
        /// 첨부파일 명
        /// </summary>
        public string fileName { get; set; }
    } 
}