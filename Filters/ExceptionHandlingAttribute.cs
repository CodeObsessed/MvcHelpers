using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using MvcHelpers.Interfaces;

namespace MvcHelpers.Filters
{
    public sealed class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        private readonly IErrorResponseModel errorResponseModel;
        private readonly IMvcLogger logService;

        public ExceptionHandlingAttribute(IErrorResponseModel errorResponseModel, IMvcLogger logService)
        {
            this.errorResponseModel = errorResponseModel;
            this.logService = logService;
        }
        /*new ErrorResponseModel(ErrorCode.InvalidOperation, Strings.InternalServerErrorMessage)
         */

        private static HttpContent GetHttpContent(IErrorResponseModel errorResponseModel)
        {
            return new ObjectContent<IErrorResponseModel>(errorResponseModel, new JsonMediaTypeFormatter());
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null)
            {
                throw new ArgumentNullException("actionExecutedContext");
            }
            var resource = actionExecutedContext.Request.RequestUri.AbsolutePath.Trim('/');
            var resourceVerb = actionExecutedContext.Request.Method.ToString();
            var logMessage = String.Format("{0} {1}", resourceVerb, resource);

            if (actionExecutedContext.Exception.GetType() == typeof(OperationCanceledException) || actionExecutedContext.Exception.GetType() == typeof(TaskCanceledException))
            {
                logService.LogWarning(actionExecutedContext.Exception, "Operation Cancelled", actionExecutedContext.ToString());
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = GetHttpContent(errorResponseModel),
                    ReasonPhrase = "Operation Cancelled"
                };
            }
            else
            {
                logService.LogCaughtException(actionExecutedContext.Exception, "Fatal exception occurred", actionExecutedContext.ToString());
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = GetHttpContent(errorResponseModel),
                    ReasonPhrase = "Exception"
                };
            }

        }
    }
}