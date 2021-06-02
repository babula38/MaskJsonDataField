using JsonMasking.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Threading.Tasks;

namespace JsonMasking
{
    public class IpWhiteListingMiddleware
    {
        private readonly RequestDelegate _next;

        public IpWhiteListingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,
                                 SensitiveAuditingFieldsContext sensitiveFieldContext,
                                 ISensitiveAuditingFields sensitiveAuditingFields)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                if (controllerActionDescriptor != null)
                {
                    string controllerName = controllerActionDescriptor.ControllerTypeInfo.Name;
                    string fieldsToMask = sensitiveAuditingFields.Get(controllerName);
                    if (!string.IsNullOrWhiteSpace(fieldsToMask))
                        sensitiveFieldContext.Values = fieldsToMask.Split(";");

                }
            }
            await _next(context);
        }
    }
}