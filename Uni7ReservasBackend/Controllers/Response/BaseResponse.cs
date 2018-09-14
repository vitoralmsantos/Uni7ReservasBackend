using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Controllers.Response
{
    public class BaseResponse
    {
        public int Status { get; set; }
        public string Detalhes { get; set; }
    }
}