using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Uni7ReservasBackend.Models;

namespace Uni7ReservasBackend
{
    public class AutenticacaoHandler : DelegatingHandler
    {
        public static string TOKEN = "TOKEN";
        public static string USERID = "USERID";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool requerValidacao = !request.RequestUri.LocalPath.Equals("api/usuario/login");

            requerValidacao = false; //TIRAR ESSA LINHA EM PRODUÇÃO

            if (requerValidacao && !ValidaToken(request))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
            return base.SendAsync(request, cancellationToken);
        }

        private bool ValidaToken(HttpRequestMessage message)
        {
            if (!message.Headers.Contains(TOKEN) || !message.Headers.Contains(USERID))
                return false;
            else
            {
                string token = message.Headers.GetValues(TOKEN).First();
                string userID = message.Headers.GetValues(USERID).First();

                if (token != null && userID != null)
                {
                    try
                    {
                        return Usuario.ValidarToken(userID, token);
                    }
                    catch(Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}