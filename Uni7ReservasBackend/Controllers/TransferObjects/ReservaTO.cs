﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Controllers.TransferObjects
{
    public class ReservaTO
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string ReservadoEm { get; set; }
        public string Horario { get; set; }
        public string Turno { get; set; }
        public string Obs { get; set; }
        public string NomeLocal { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public List<string> Equipamentos { get; set; }
    }
}