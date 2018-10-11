using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Controllers.Response
{
    public class TokenResponse : BaseResponse
    {
        public string Token { get; set; }
        public string UserID { get; set; }
    }
}