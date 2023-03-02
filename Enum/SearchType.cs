using System.ComponentModel;

namespace board_dotnet.Enum;

public enum SearchType 
{
    /// <summary>
    /// 제목
    /// </summary>
    [field:Description("제목")]
    title,

    /// <summary>
    /// 내용
    /// </summary>
    [field:Description("내용")]
    content
    // [field:Description("닉네임")]
    // nickname
}