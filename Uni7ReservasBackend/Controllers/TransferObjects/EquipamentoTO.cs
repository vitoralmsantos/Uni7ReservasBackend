using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Controllers.TransferObjects
{
    public class EquipamentoTO
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public bool Disponivel { get; set; }
        public int IdCategoria { get; set; }
    }
}