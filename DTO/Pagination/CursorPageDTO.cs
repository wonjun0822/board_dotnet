namespace board_dotnet.DTO;

public sealed record CursorDTO<T>(
    T data,
    long cursor,
    bool lastPage
);
