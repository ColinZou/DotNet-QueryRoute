using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FunCode.QueryRoute
{
    public class RouteParamAttribute : FromQueryAttribute, IParameterModelConvention
    {
        public RouteParamAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public void Apply(ParameterModel parameter)
        {
            if (parameter.Action.Selectors != null && parameter.Action.Selectors.Any())
            {
                string paramName =
                    parameter.BindingInfo?.BinderModelName ?? parameter.ParameterName;
                parameter
                    .Action
                    .Selectors
                    .Last()
                    .ActionConstraints
                    .Add(new RouteParamActionConstraint(paramName, Value));
            }
        }
    }
}
