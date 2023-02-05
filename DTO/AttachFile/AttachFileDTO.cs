namespace board_dotnet.DTO
{
    //[SwaggerExclude]
    public record AttachFileDTO {
        public long articleId { get; set; }
        public long attachFileId { get; set; }
        public string fileName { get; set; }
    } 
}