namespace DwellingAPI.Middlewares.ApplicationBuilderExtension
{
    public static class AppBuilderExtension
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
