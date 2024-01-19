using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace FunCode.QueryRoute.NSwag
{
    public class RouteParamOperationProcessor : IOperationProcessor
    {
        private static readonly Type _attrType = typeof(RouteParamAttribute);

        bool IOperationProcessor.Process(OperationProcessorContext context)
        {
            // getting all parameters with _attrType from method info
            List<KeyValuePair<string, RouteParamAttribute>> parameters = context
                .MethodInfo
                .GetParameters()
                .Where(item => item.GetCustomAttributes(_attrType, false).Any())
                .Select(item =>
                {
                    RouteParamAttribute? attr =
                        item.GetCustomAttributes(_attrType, false).First() as RouteParamAttribute;
                    return KeyValuePair.Create(item.Name!, attr!);
                })
                .ToList();
            if (parameters.Count == 0)
            {
                return true;
            }
            // getting all route param names as list
            List<string> paramNameList = parameters.Select(item => item.Key).ToList();
            // getting all query params bind to openapi operation
            IList<global::NSwag.OpenApiParameter> apiParams = context
                .OperationDescription
                .Operation
                .Parameters;
            // removing all route params bind to openapi operation
            apiParams
                .Where(item => paramNameList.Contains(item.Name))
                .ToList()
                .ForEach(item => apiParams.Remove(item));
            // add query string to path
            context.OperationDescription.Path +=
                "?" + string.Join("&", parameters.Select(item => $"{item.Key}={item.Value.Value}"));
            return true;
        }
    }
}
