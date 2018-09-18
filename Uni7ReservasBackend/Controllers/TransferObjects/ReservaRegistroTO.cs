using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Controllers.TransferObjects
{
    public class ReservaRegistroTO
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string ReservadoEm { get; set; }
        public string Horario { get; set; }
        public string Turno { get; set; }
        public string Obs { get; set; }
        public int IdUsuario { get; set; }
        public int IdLocal { get; set; }
        public int IdEquipamento1 { get; set; }
        public int IdEquipamento2 { get; set; }
    }
}