namespace Contracts.Activity.Requests;

public class EditActivityRequest : BaseActivityRequest
{
    public string Id { get; set; } = string.Empty;
}