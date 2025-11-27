namespace Events.Application.Extensions;

public static class ValidationErrorsMapper
{
    public static Dictionary<string, string[]> ToDictionary(this List<FluentValidation.Results.ValidationFailure> validationFailures)
    {
        Dictionary<string, string[]> errors = new Dictionary<string, string[]>();
        if (validationFailures.Any())
        {
            var groupedErrors = validationFailures
           .GroupBy(e => e.PropertyName).ToList();

            foreach (var group in groupedErrors)
            {
                errors.Add(group.Key, group.Select(e => e.ErrorMessage).ToArray());
            }
        }
        return errors;
    }
}
