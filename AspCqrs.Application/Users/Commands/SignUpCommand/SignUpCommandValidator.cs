using System.Linq;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Rules;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.Users.Commands.SignUpCommand
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator(IApplicationDbContext context)
        {
            RuleFor(command => command.Password)
                .Required("Password")
                .MinimumLength(8)
                .WithMessage("Password must be at 8 character at least.")
                .Must(s => s.Any(char.IsDigit))
                .WithMessage("Password must contain one digit at least")
                .Must(s => s.Any(c => s.Count(countChar => countChar == c) == 1))
                .WithMessage("Password must contain one unique character at least");

            RuleFor(command => command.UserName)
                .Required("Username")
                .MustAsync((s, token) => context.DomainUsers.AllAsync(u => u.UserName != s, token))
                .WithMessage((_, s) => $"Username '{s}' is already in use.");
        }
    }
}