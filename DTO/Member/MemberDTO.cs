namespace board_dotnet.DTO
{
    //[SwaggerExclude]
    public record MemberDTO {
        public long id { get; set; }
        public string email { get; set; } = string.Empty;
        public string nickname { get; set; } = string.Empty;
    }
}