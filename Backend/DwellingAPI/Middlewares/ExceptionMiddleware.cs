using DwellingAPI.DAL.Entities;
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
