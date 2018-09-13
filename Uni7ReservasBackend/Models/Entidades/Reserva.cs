using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uni7ReservasBackend.Models.Entidades;

namespace Uni7ReservasBackend.Models
{
    public partial class Reserva
    {
        public static void ReservarLocal(DateTime data, string horario, string turno, int idLocal,
            string obs)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                //Consulta local
                var local_ = from Local l in context.Locais
                             where l.Id == idLocal
                             select l;

                if (local_.Count() == 0)
                {
                    throw new EntityException(EntityExcCode.LOCALINEXISTENTE, idLocal.ToString());
                }

                //Verifica disponibilidade do local
                var reserva_ = from Reserva r in context.Reservas
                               where r.Data.Equals(data) && r.Turno.Equals(turno) && r.Horario.Equals(horario)
                                 && r.Local.Id == idLocal
                               select r.Local.Id;

                if (reserva_.Count() > 0)
                {
                    throw new EntityException(EntityExcCode.LOCALINDISPONIVEL, "");
                }

                //Realiza a reserva
                Reserva reserva = new Reserva();
                reserva.Data = data;
                reserva.Turno = turno;
                reserva.Horario = horario;
                reserva.Obs = obs;
                reserva.Local = local_.First();

                context.SaveChanges();
            }
        }

        //INCLUIR DOIS EQUIPAMENTOS NOS PARÂMETROS
        public static void ReservarLocal(DateTime data, string horario, string turno, int idLocal, 
            string obs, int idCatEquipamento)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                //Consulta local
                var local_ = from Local l in context.Locais
                                where l.Id == idLocal
                                select l;

                if (local_.Count() == 0)
                {
                    throw new EntityException(EntityExcCode.LOCALINEXISTENTE, idLocal.ToString());
                }

                //Consulta categoria do equipamento
                var catequip_ = from CategoriaEquipamento ce in context.Categorias
                                where ce.Id == idCatEquipamento
                                select ce;

                if (catequip_.Count() == 0)
                {
                    throw new EntityException(EntityExcCode.CATEGORIAINEXISTENTE, idCatEquipamento.ToString());
                }

                //Verifica disponibilidade do local
                var reserva_ = from Reserva r in context.Reservas
                              where r.Data.Equals(data) && r.Turno.Equals(turno) && r.Horario.Equals(horario)
                                && r.Local.Id == idLocal
                              select r.Local.Id;

                if (reserva_.Count() > 0)
                {
                    throw new EntityException(EntityExcCode.LOCALINDISPONIVEL, "");
                }

                //Verifica disponibilidade do equipamento
                var equip_ = from Equipamento e in context.Equipamentos
                             where e.CategoriaEquipamento.Id == idCatEquipamento && e.Disponivel
                             select e;

                if (equip_.Count() == 0)
                {
                    throw new EntityException(EntityExcCode.EQUIPAMENTOINDISPONIVEL, "");
                }

                //Realiza a reserva
                Reserva reserva = new Reserva();
                reserva.Data = data;
                reserva.Turno = turno;
                reserva.Horario = horario;
                reserva.Obs = obs;
                reserva.Local = local_.First();
                reserva.CategoriasEquipamentos.Add(catequip_.First());

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Locais disponíveis para reserva.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="horario"></param>
        /// <param name="turno"></param>
        /// <param name="reservaveis">Somente os reserváveis (true) ou os não reserváveis (false)</param>
        /// <returns></returns>
        public List<Local> ConsultarLocaisDisponiveis(DateTime data, string horario, string turno, bool reservaveis)
        {
            List<Local> Locais = new List<Local>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                if (reservaveis)
                {
                    var local1_ = from Local l in context.Locais
                                 where l.Disponivel && l.Reservavel &&
                                 !(from Reserva r in context.Reservas
                                   where r.Data.Equals(data) && r.Turno.Equals(turno) && r.Horario.Equals(horario)
                                   select r.Local.Id).Contains(l.Id)
                                 select l;

                    Locais = local1_.ToList();
                }
                else
                {
                    var local2_ = from Local l in context.Locais
                                 where l.Disponivel && !l.Reservavel
                                 select l;

                    Locais = local2_.ToList();
                }
            }

            return Locais;
        }

    }
}