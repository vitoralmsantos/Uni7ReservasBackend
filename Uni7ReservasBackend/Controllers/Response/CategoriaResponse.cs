using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uni7ReservasBackend.Controllers.TransferObjects;

namespace Uni7ReservasBackend.Controllers.Response
{
    public class CategoriaResponse : BaseResponse
    {
        public CategoriaTO Categoria { get; set; }
    }
}