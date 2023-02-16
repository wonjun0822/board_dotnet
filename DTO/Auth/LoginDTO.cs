    namespace board_dotnet.DTO;
    
    public record LoginDTO {
        public string id { get; set; } = string.Empty;
        public string pwd { get; set; } = string.Empty;
    }