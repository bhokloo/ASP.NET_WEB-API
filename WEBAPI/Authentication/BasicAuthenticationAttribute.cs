using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WEBAPI.Authentication
{
    //this is a validation filter for WEB APIs Req and res objects.
    //in WEB API. BasicAuthentication, the clients sends the credentials using HEADERS...so use actionCOntext to use that. 
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {

                if (actionContext.Request.Headers.Authorization == null)
                {

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
                else
                {
                    string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                    string decodedAuhenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                    string[] usernamePassword = decodedAuhenticationToken.Split(':');
                    string username = usernamePassword[0];
                    string password = usernamePassword[1];

                    if (UserSecurity.Login(username, password))
                    {
                        //assigning the value of name to thread....here currentPrincipal is derieved from ClaimsPrincipal..
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}