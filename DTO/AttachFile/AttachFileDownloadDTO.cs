namespace board_dotnet.DTO;

public class AttachFileDownloadDTO
{
    public byte[] content { get; set; }
    public string contentType { get; set; }
    public string fileName { get; set; }
}