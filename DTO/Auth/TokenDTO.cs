namespace board_dotnet.DTO
{
    //[SwaggerExclude]
    public record TokenDTO {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }
}