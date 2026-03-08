using Contracts.Common;

namespace Contracts.Activity.Requests;

public class ActivityParamsRequest : PaginationParamsRequest<DateTimeOffset>
{
    public string? Filter { get; set; }
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;
}