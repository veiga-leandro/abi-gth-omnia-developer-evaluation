namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class AuthExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // After execution, check for specific status codes
                if (context.Response.StatusCode == 401)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Unauthorized. Please login to access this resource."
                    });
                }
                else if (context.Response.StatusCode == 403)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Forbidden. You don't have permission to access this resource."
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
