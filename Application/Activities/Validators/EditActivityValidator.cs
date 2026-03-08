using Application.Activities.Commands;
using Contracts.Activity.Requests;
using FluentValidation;

namespace Application.Activities.Validators;

public class EditActivityValidator : BaseActivityValidator<EditActivity.Command, EditActivityRequest>
{
    public EditActivityValidator(): base(x => x.Activity)
    {
        RuleFor(x => x.Activity.Id)
            .NotEmpty().WithMessage("Activity id is required");
    }
}