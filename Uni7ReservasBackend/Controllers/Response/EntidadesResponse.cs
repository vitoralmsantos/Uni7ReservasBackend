using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uni7ReservasBackend.Controllers.TransferObjects;

namespace Uni7ReservasBackend.Controllers.Response
{
    public class EntidadesResponse<T> : BaseResponse
    {
        public List<T> Elementos { get; set; }

        public EntidadesResponse()
        {
            Elementos = new List<T>();
        }
    }
}