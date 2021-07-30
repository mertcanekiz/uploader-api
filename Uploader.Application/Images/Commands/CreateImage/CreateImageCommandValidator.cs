using System.IO;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Uploader.Application.Images.Commands.CreateImage
{
    public class CreateImageCommandValidator : AbstractValidator<CreateImageCommand>
    {
        public CreateImageCommandValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .NotEmpty();

            RuleFor(x => x.File).NotNull().SetValidator(new FileTypeValidator());
        }

        class FileTypeValidator : AbstractValidator<IFormFile>
        {
            public FileTypeValidator()
            {
                RuleFor(x => x.FileName)
                    .Matches("^.+[jpg|png]$")
                    .WithMessage(((file, s) =>
                    {
                        var extension = Path.GetExtension(s);
                        return $"Filetype {extension} is not supported.";
                    }));
            }
        }
    }
}