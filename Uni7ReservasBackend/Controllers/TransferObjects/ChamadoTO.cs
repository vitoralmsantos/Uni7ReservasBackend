using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Controllers.TransferObjects
{
    public class ChamadoTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int Status { get; set; }
        public string Observacoes { get; set; }
        public string DataLimite { get; set; }
        public string DataPrevista { get; set; }
        public string Telefone { get; set; }
    }
}