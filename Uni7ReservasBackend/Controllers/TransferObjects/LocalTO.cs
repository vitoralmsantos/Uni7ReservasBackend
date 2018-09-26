using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Controllers.TransferObjects
{
    public class LocalTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Disponivel { get; set; }
        public bool Reservavel { get; set; }
        public int Tipo { get; set; }
    }
}