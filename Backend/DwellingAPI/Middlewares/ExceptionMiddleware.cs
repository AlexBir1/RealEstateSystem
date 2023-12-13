using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Exceptions;
using DwellingAPI.ResponseWrapper.Implementation;

namespace DwellingAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationFailedException ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    await context.Response.WriteAsJsonAsync(new ResponseWrapper<string>(new List<string> { ex.Message }));
                else
                    await context.Response.WriteAsJsonAsync(new ResponseWrapper<string>(ex.Errors));
            }
            catch (Exception ex)
            {
                var currentException = ex;

                var errors = new List<string>
                {
                    new string(currentException.Message)
                };

                while (currentException.InnerException != null)
                {
                    currentException = currentException.InnerException;
                    errors.Add(currentException.Message);
                }

                await context.Response.WriteAsJsonAsync(new ResponseWrapper<string>(errors));
            }
        }
    }
}
