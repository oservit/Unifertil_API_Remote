using Libs.Common;
using Libs.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace API.Remote.Controllers.Base
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var result = new DataResult
            {
                Data = null,
                Success = false,
                Message = "An unexpected error occurred."
            };

            try
            {
                if (context.Exception is NotFoundException notFoundEx)
                {
                    result.Message = notFoundEx.Message;
                    context.Result = new NotFoundObjectResult(result);
                }
                else if (context.Exception is BusinessException businessEx)
                {
                    result.Message = businessEx.Message;
                    context.Result = new BadRequestObjectResult(result);
                }
                else
                {
                    result.Message = $"{context.Exception.Message}\nStackTrace: {context.Exception.StackTrace}";
                    if (context.Exception.InnerException != null)
                    {
                        result.Message += $"\nInnerException: {context.Exception.InnerException.Message}\nInnerException StackTrace: {context.Exception.InnerException.StackTrace}";
                    }
                    context.Result = new ObjectResult(result)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                // Optional: Handle any exceptions that occur while handling the original exception
                result.Message = "An error occurred while processing the error.";
                context.Result = new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }

}
