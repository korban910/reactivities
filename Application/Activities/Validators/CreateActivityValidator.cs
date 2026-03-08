using Application.Activities.Commands;
using Contracts.Activity.Requests;

namespace Application.Activities.Validators;

public class CreateActivityValidator : BaseActivityValidator<CreateActivity.Command, CreateActivityRequest>
{
    public CreateActivityValidator(): base(x => x.Activity)
    {
    }
}