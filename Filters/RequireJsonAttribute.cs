using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using MvcHelpers.Interfaces;
using MvcHelpers.Models;

namespace MvcHelpers.Filters
{
    public sealed class RequireJsonAttribute : AuthorizationFilterAttribute
    {
        private readonly IErrorResponseModel errorResponseModel;
        /*
         * ErrorCode.InvalidHeaders, Strings.InvalidHeadersErrorMessage
         */

        public RequireJsonAttribute(IErrorResponseModel errorResponseModel)
        {
            this.errorResponseModel = errorResponseModel;
        }


        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!(actionContext.Request.Headers.Accept.ToString().Contains("application/json") || actionContext.Request.Headers.Accept.ToString().Contains("application/*") || actionContext.Request.Headers.Accept.ToString().Contains("*/*")))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorResponseModel);
            }
            else if (actionContext.Request.Content.Headers.Contains("Content-Type") && !(actionContext.Request.Content.Headers.ContentType.ToString().Contains("application/json") || actionContext.Request.Content.Headers.ContentType.ToString().Contains("application/*") || actionContext.Request.Content.Headers.ContentType.ToString().Contains("*/*")))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorResponseModel);
            }
        }
    }
}