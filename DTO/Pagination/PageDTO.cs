namespace board_dotnet.DTO;

public sealed record OffsetDTO<T>(
    T data,
    long pageIndex,
    int pageSize,
    int totalPage
);
