namespace board_dotnet.DTO
{
    //[SwaggerExclude]
    public record MemberDTO {
        public string nickname { get; set; } = string.Empty;
    }

    //[SwaggerExclude]
    public record LoginDTO {
        public string id { get; set; } = string.Empty;
        public string pwd { get; set; } = string.Empty;
    }
}