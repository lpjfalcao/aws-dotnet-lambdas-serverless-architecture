using FluentValidation.Results;

namespace Curriculo4Dev.Core.Application.Extensions
{
    public static class FluentValidationExtensions
    {
        public static string GetErrorMessage(this List<ValidationFailure> errors)
        {
            return string.Join(", ", errors);            
        }
    }
}
