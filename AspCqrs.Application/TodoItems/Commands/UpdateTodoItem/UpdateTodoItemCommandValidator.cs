using FluentValidation;

namespace AspCqrs.Application.TodoItems.Commands.UpdateTodoItem
{
    public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
    {
        public UpdateTodoItemCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty()
                .MaximumLength(64)
                .MinimumLength(8);
        }
    }
}