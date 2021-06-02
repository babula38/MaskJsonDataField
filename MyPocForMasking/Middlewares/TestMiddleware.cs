using System.Threading.Tasks;
using JsonMasking.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MyPocForMasking.Middlewares
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, ISanitizeDataService sanitizeDataService, SensitiveAuditingFieldsContext sensitiveAuditingFields)
        {
            var obj = new { Password = "password" };

            // var fields = sanitizeDataService.Sanitize(obj);
            var fields = obj.MaskFields(sensitiveAuditingFields.Values);

            await _next(context);
        }
    }

    public static class TestMiddle
    {
        public static IApplicationBuilder UseTestMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<TestMiddleware>();

            return app;
        }
    }
}
