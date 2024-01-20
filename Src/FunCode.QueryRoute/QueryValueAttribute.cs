using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FunCode.QueryRoute
{
    public class QueryValueAttribute : FromQueryAttribute, IParameterModelConvention
    {
        public QueryValueAttribute(string value)
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
                    .Add(new QueryValueActionConstraint(paramName, Value));
            }
        }
    }
}
