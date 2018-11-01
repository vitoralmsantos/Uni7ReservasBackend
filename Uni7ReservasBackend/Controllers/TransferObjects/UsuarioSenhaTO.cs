using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Controllers.TransferObjects
{
    public class UsuarioSenhaTO
    {
        public int Id { get; set; }
        public string SenhaAntiga { get; set; }
        public string SenhaNova { get; set; }
    }
}