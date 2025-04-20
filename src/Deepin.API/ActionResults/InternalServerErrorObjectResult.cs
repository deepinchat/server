using Microsoft.AspNetCore.Mvc;

namespace Deepin.API.ActionResults;
public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object? value) : base(value)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}