using FluentValidation;
using ToDoApp.API.ViewModels;

namespace ToDoApp.API.Validators
{
    public class CreateToDoItemViewModelValidator : AbstractValidator<CreateToDoItemViewModel>
    {
        public CreateToDoItemViewModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.DueDate)
                .NotNull().WithMessage("Due date is required.")
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Due date cannot be in the past.");

            RuleFor(x => x.Priority)
                .NotNull().WithMessage("Priority is required.")
                .IsInEnum().WithMessage("Invalid priority value.");

            RuleFor(x => x.Status)
                .NotNull().WithMessage("Status is required.")
                .IsInEnum().WithMessage("Invalid status value.");
        }
    }
}
