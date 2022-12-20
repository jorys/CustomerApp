using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace CustomerApp.RestApi.Common;

[ApiController]
public abstract class InternalControllerBase : ControllerBase
{
    readonly ApiBehaviorOptions _apiBehaviorOptions;

    protected InternalControllerBase(IOptions<ApiBehaviorOptions> apiBehaviorOptions)
    {
        _apiBehaviorOptions = apiBehaviorOptions.Value;
    }

    protected ActionResult ToActionResult<TResult>(ErrorOr<TResult> errorOrResult, Func<TResult, ActionResult> resultToActionResult)
    {
        return errorOrResult.Match(
            result => resultToActionResult(result),
            errors =>
            {
                // Get first error
                var firstError = errors.First();

                // Map to status code
                var statusCode = firstError.Type switch
                {
                    ErrorType.NotFound => (int)HttpStatusCode.NotFound,
                    ErrorType.Validation => (int)HttpStatusCode.BadRequest,
                    ErrorType.Conflict => (int)HttpStatusCode.Conflict,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                // Initialize ProblemDetails with default mappings
                var problemDetails = new ProblemDetails { Status = statusCode };
                if (_apiBehaviorOptions.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
                {
                    problemDetails.Title ??= clientErrorData.Title;
                    problemDetails.Type ??= clientErrorData.Link;
                }

                // Add trace id
                var traceId = HttpContext?.TraceIdentifier;
                if (traceId != null)
                {
                    problemDetails.Extensions["traceId"] = traceId;
                }

                // Keep only error code and description
                var formattedErrors = errors.Select(error => new
                {
                    error.Code,
                    error.Description
                });
                problemDetails.Extensions.Add("errors", formattedErrors);

                return new ObjectResult(problemDetails);
            });
    }
}