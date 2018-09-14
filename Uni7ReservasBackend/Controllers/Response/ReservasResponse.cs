using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uni7ReservasBackend.Controllers.TransferObjects;

namespace Uni7ReservasBackend.Controllers.Response
{
    public class ReservasResponse : BaseResponse
    {
        public List<ReservaTO> Reservas { get; set; }
    }
}