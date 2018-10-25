using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Controllers.TransferObjects
{
    public class CategoriaTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string ComentarioReserva { get; set; }
    }
}