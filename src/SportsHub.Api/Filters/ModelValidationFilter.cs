using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace SportsHub.Api.Filters;

/// <summary>
/// Action filter for automatic model validation
/// </summary>
public class ModelValidationFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => ConvertToSnakeCase(kvp.Key),
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new
            {
                error = new
                {
                    message = "Validation failed",
                    validation_errors = errors,
                    timestamp = DateTime.UtcNow
                }
            };

            context.Result = new BadRequestObjectResult(response);
        }

        base.OnActionExecuting(context);
    }

    private static string ConvertToSnakeCase(string str)
    {
        return JsonNamingPolicy.SnakeCaseLower.ConvertName(str);
    }
} 