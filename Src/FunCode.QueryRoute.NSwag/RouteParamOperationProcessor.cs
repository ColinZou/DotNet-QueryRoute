using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace FunCode.QueryRoute.NSwag
{
    public class RouteParamOperationProcessor : IOperationProcessor
    {
        private static readonly Type _attrType = typeof(QueryValueAttribute);
        private static readonly char _questioMark = '?';
        private static readonly char _andSign = '&';

        bool IOperationProcessor.Process(OperationProcessorContext context)
        {
            // getting all parameters with _attrType from method info
            List<KeyValuePair<string, string>> parameters = context
                .MethodInfo
                .GetParameters()
                .Where(item => item.GetCustomAttributes(_attrType, false).Any())
                .Select(item =>
                {
                    QueryValueAttribute? attr =
                        item.GetCustomAttributes(_attrType, false).First() as QueryValueAttribute;
                    return KeyValuePair.Create(item.Name!, attr!.Value);
                })
                .ToList();
            if (parameters.Count == 0)
            {
                return true;
            }
            string routePath = context.OperationDescription.Path;
            string pendingPart = string.Join(_andSign, parameters.Select(item => $"{item.Key}={item.Value}"));
            if (routePath.Contains(pendingPart)) {
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
            bool pathHasQuestionMark = routePath.Contains(_questioMark);
            // add query string to path
            context.OperationDescription.Path +=
                (pathHasQuestionMark ? _andSign : _questioMark)
                + pendingPart;
            return true;
        }
    }
}
