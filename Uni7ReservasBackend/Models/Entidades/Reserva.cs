using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uni7ReservasBackend.Models
{
    public partial class Reserva
    {
        public static void Cadastrar(DateTime data, string horario, string turno, int idLocal, int idCatEquipamento)
        {

        }

        public bool ConsultarDisponibilidadeLocal(DateTime data, string horario, string turno, int idLocal)
        {
            bool disponivel = false;



            return disponivel;
        }
        
    }
}