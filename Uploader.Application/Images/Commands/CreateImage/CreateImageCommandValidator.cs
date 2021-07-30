using FluentValidation;

namespace Uploader.Application.Images.Commands.CreateImage
{
    public class CreateImageCommandValidator : AbstractValidator<CreateImageCommand>
    {
        public CreateImageCommandValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .NotEmpty();

            RuleFor(x => x.File.FileName)
                .Matches("^.+[jpg|png]$");
        }
    }
}