using FluentValidation;

namespace Uploader.Application.Images.Commands.UpdateImageDescription
{
    public class UpdateImageDescriptionValidator : AbstractValidator<UpdateImageDescriptionCommand>
    {
        public UpdateImageDescriptionValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}