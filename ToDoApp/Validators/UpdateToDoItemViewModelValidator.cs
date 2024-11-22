using FluentValidation;
using ToDoApp.API.ViewModels;

namespace ToDoApp.API.Validators
{
    public class UpdateToDoItemViewModelValidator : AbstractValidator<UpdateToDoItemViewModel>
    {
        public UpdateToDoItemViewModelValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Due date cannot be in the past.");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority value.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value.");
        }
    }
}
