using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Primitives;

namespace FunCode.QueryRoute
{
    public class RouteParamActionConstraint : IActionConstraint
    {
        private readonly string _paramName;
        private readonly string _paramValue;
        public int Order => 999;

        public RouteParamActionConstraint(string name, string value)
        {
            _paramName = name.Trim();
            _paramValue = value.Trim().ToLower(System.Globalization.CultureInfo.CurrentCulture);
        }

        public bool Accept(ActionConstraintContext context)
        {
            IQueryCollection query = context.RouteContext.HttpContext.Request.Query;
            bool foundParameter = query.ContainsKey(_paramName);
            if (!foundParameter)
            {
                return false;
            }
            bool emptyValue = _paramValue.Length == 0;
            if (emptyValue)
            {
                return true;
            }
            bool gotParamValue = query.TryGetValue(_paramName, out StringValues queryValue);
            if (!gotParamValue || !queryValue.Any())
            {
                return false;
            }
            string paramValue = queryValue
                .First()
                .ToLower(System.Globalization.CultureInfo.CurrentCulture);
            return paramValue == _paramValue;
        }
    }
}
