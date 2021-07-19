using FluentValidation;

namespace AspCqrs.Application.TodoItems.Commands.CreateTodoItem
{
    public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
    {
        public CreateTodoItemCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty()
                .MaximumLength(64)
                .MinimumLength(8);

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}