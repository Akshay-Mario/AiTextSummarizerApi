

using Microsoft.AspNetCore.Authorization;

//this is just for reference
namespace AiTextSummarizerApi.Middlewares
{
    public class CustomJwtValidatorMiddleware
    {

        private readonly RequestDelegate _next;

        public CustomJwtValidatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoints = context.GetEndpoint();
            if (endpoints?.Metadata?.GetMetadata<IAllowAnonymous>() is not null)
            {
                await _next(context);
                return;
            }

            if (endpoints?.Metadata?.GetMetadata<IAuthorizeData>() is not null)
            {
                var user = context.User;

            }
        }

    }
}