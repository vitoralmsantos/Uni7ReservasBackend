using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Controllers.TransferObjects
{
    public class RecursoTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Detalhes { get; set; }
        public string Vencimento { get; set; }
        public int Tipo { get; set; }
    }
}