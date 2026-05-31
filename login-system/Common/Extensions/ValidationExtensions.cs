using FluentValidation.Results;

namespace login_system.Common.Extensions
{
    public static class ValidationExtensions
    {
        public static Dictionary<string, string[]> ToValidationDictionary(this ValidationResult validationResult)
        {
            return validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );
        }
    }
}