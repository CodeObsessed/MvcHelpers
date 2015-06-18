using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.WindowsAzure.ServiceRuntime;
using Newtonsoft.Json;
using MvcHelpers.Interfaces;
using MvcHelpers.Utility;
using MvcHelpers.Validation;

namespace MvcHelpers.Filters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class ValidateModelAttribute : ActionFilterAttribute
    {
        private readonly IErrorValidationResponseModel errorValidationResponseModel;

        public ValidateModelAttribute(IErrorValidationResponseModel errorValidationResponseModel)
        {
            this.errorValidationResponseModel = errorValidationResponseModel;
        }

        private static void ValidateGroupModels(HttpActionContext actionContext)
        {
            if (!actionContext.ActionArguments.Any())
                return;

            var multiModelAttributes = new List<MultiModelPropertyAttribute>();

            foreach (var modelPair in actionContext.ActionArguments)
            {
                var model = modelPair.Value;

                if (model == null)
                {
                    throw new ArgumentNullException("actionContext","Model of controller cannot be nullable");
                }

                foreach (var property in model.GetType().GetProperties().Where(x => x.CanRead))
                {
                    var multiModelProperties = property.GetCustomAttributes<MultiModelPropertyAttribute>().ToList();

                    if (!multiModelProperties.Any())
                    {
                        continue;
                    }

                    var requiredModelProperties = multiModelProperties.Where(x => x.Required).Select(
                        x =>
                        {
                            x.IsValid(property.GetValue(model, null));
                            return x;
                        });

                    multiModelAttributes.AddRange(requiredModelProperties);
                }
            }


            var groupedModelAttrbutes = multiModelAttributes.GroupBy(x => x.ModelType).ToList();

            var validGroup = !groupedModelAttrbutes.Any() || groupedModelAttrbutes.ToList().Any(modelGroups => modelGroups.All(attribute => attribute.Valid));

            if (validGroup) return;

            foreach (var property in groupedModelAttrbutes.SelectMany(modelOption => modelOption))
            {
                actionContext.ModelState.AddModelError(property.PropertyName, "Option " + property.ModelType + ": Property '" + property.PropertyName + "' is required.");
            }
        }

        /// <summary>
        /// new ErrorResponseModel(ErrorCode.InvalidInputModel, Strings.InvalidInputModelMessage)
        /// new ErrorValidationResponseModel(ErrorCode.InvalidInputModel, Strings.InvalidInputModelMessage, actionContext.ModelState)
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            
            ValidateGroupModels(actionContext);
            
            if (actionContext.ModelState.IsValid == false)
            {
                errorValidationResponseModel.SetModelState(actionContext.ModelState);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorValidationResponseModel);
            }

        }
    }
}