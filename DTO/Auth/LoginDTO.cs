    namespace board_dotnet.DTO;
    
    public record LoginDTO {
        /// <summary>
        /// 사용자 Email
        /// </summary>
        public string email { get; set; } = string.Empty;

        /// <summary>
        /// 사용자 Password
        /// </summary>
        public string pwd { get; set; } = string.Empty;
    }