namespace Contracts.Common;

public class PaginationParamsRequest<TCursor>
{
    private const int MaxPageSize = 50;
    
    public TCursor? Cursor { get; set; }
    private int _pageSize = 3;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? value : MaxPageSize;
    }
}