using System.Linq;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Rules;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.Users.Commands.SignInCommand
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator(IApplicationDbContext context)
        {
            RuleFor(command => command.UserName)
                .Required("Username")
                .MustAsync((s, token) => context.DomainUsers
                    .Select(u => u.UserName)
                    .ContainsAsync(s, token))
                .WithMessage((_, s) => $"User '{s}' does not exist.");

            RuleFor(command => command.Password)
                .Required("Password");
        }
    }
}