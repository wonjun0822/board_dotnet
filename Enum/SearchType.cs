using System.ComponentModel;

namespace board_dotnet.Enum;

public enum SearchType 
{
    [field:Description("제목")]
    title,
    [field:Description("내용")]
    content
    // [field:Description("닉네임")]
    // nickname
}