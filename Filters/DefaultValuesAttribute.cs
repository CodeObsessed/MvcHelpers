using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace MvcHelpers.Filters
{
    public sealed class DefaultValuesAttribute : ActionFilterAttribute
    {
        static public void SetDefaults(object self)
        {
            if (self == null)
                return;

            var baseType = self.GetType().BaseType;
            var baseProps = baseType != null ? baseType.GetProperties() : new PropertyInfo[0];

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(self))
            {
                var skip = baseProps.Any(info => prop.Name == info.Name);

                if (skip)
                    continue;

                var attr = prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;

                if (attr == null)
                    continue;

                if (self.GetType().GetProperty(prop.Name).GetValue(self, null) == null)
                    prop.SetValue(self, attr.Value);
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            foreach (var model in actionContext.ActionArguments.Select(actionArgument => actionArgument.Value))
            {
                SetDefaults(model);
            }
        }
    }
}
