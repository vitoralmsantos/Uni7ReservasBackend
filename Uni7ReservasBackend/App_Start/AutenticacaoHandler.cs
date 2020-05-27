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
        public static string TOKEN = "app_token";
        public static string USERID = "user_id";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Se não for login e GET então precisa do token
            bool requerValidacao = (!request.Method.Method.Equals("GET") && 
                !request.RequestUri.LocalPath.Equals("/api/usuario/login") &&
                !request.RequestUri.LocalPath.Equals("/api/usuario/enviarsenha") &&
                !request.RequestUri.LocalPath.Equals("/api/usuario/solicitarcadastro")
                );

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
            string token = "";
            string userid = "";
            var queryString = message.GetQueryNameValuePairs();
            foreach(KeyValuePair<string, string> kvp in queryString)
            {
                //if (kvp.Key.Equals(TOKEN) && kvp.Value != null) token = kvp.Value;
                if (kvp.Key.Equals(USERID) && kvp.Value != null) userid = kvp.Value;
            }
            token = message.RequestUri.ToString().Substring(message.RequestUri.ToString().IndexOf(TOKEN) + 11, 32);

            if (token.Length == 0 || userid.Length == 0)
                return false;
            else
            {
                try
                {
                    token = token.Replace("\"", "");
                    int userID = Convert.ToInt32(userid);
                    return Usuario.ValidarToken(userID, token);
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
        }
    }
}