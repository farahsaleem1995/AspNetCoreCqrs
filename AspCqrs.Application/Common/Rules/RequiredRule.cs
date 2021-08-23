using FluentValidation;

namespace AspCqrs.Application.Common.Rules
{
    public static class RequiredRule
    {
        public static IRuleBuilderOptions<T, TProp> Required<T, TProp>(this IRuleBuilder<T, TProp> builder, string fieldName)
        {
            return builder
                .NotEmpty()
                .WithMessage($"Field '{fieldName}' cannot be empty.");
        }
    }
}