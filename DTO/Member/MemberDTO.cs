namespace board_dotnet.DTO
{
    //[SwaggerExclude]
    public record MemberDTO {
        public string id { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string nickname { get; set; } = string.Empty;
    }
}