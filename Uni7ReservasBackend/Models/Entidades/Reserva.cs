using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uni7ReservasBackend.Models.Entidades;

namespace Uni7ReservasBackend.Models
{
    public partial class Reserva
    {
        public static void Reservar(DateTime data, string horario, string turno, int idLocal,
            string obs, int idCatEquipamento1, int idCatEquipamento2)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                //Consulta local
                var local_ = from Local l in context.Locais.Include("RestricoesCategoriaEquipamento")
                             where l.Id == idLocal
                             select l;

                if (local_.Count() == 0)
                {
                    throw new EntidadesException(EntityExcCode.LOCALINEXISTENTE, idLocal.ToString());
                }
                else if (!local_.First().Reservavel && idCatEquipamento1 == 0 && idCatEquipamento2 == 0)
                {
                    throw new EntidadesException(EntityExcCode.EQUIPAMENTONECESSARIO, "");
                }
                else if (idCatEquipamento1 == idCatEquipamento2)
                {
                    throw new EntidadesException(EntityExcCode.EQUIPAMENTOSIGUAIS, "");
                }

                IEnumerable<CategoriaEquipamento> catequip1_ = null;
                if (idCatEquipamento1 > 0)
                {
                    //Consulta categoria do equipamento 1
                    catequip1_ = from CategoriaEquipamento ce in context.Categorias
                                 where ce.Id == idCatEquipamento1
                                 select ce;

                    if (catequip1_.Count() == 0)
                    {
                        throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, idCatEquipamento1.ToString());
                    }

                    //Verifica disponibilidade do equipamento 1
                    var equip1_ = from Equipamento e in context.Equipamentos
                                  where e.CategoriaEquipamento.Id == idCatEquipamento1 && e.Disponivel
                                  select e;

                    if (equip1_.Count() == 0)
                    {
                        throw new EntidadesException(EntityExcCode.EQUIPAMENTOINDISPONIVEL, catequip1_.First().Nome);
                    }

                    if (local_.First().RestricoesCategoriaEquipamento.Contains(catequip1_.First()))
                    {
                        throw new EntidadesException(EntityExcCode.RESTRICAOLOCALEQUIPAMENTO, catequip1_.First().Nome);
                    }
                }

                IEnumerable<CategoriaEquipamento> catequip2_ = null;
                if (idCatEquipamento2 > 0)
                {
                    //Consulta categoria do equipamento 2
                    catequip2_ = from CategoriaEquipamento ce in context.Categorias
                                 where ce.Id == idCatEquipamento2
                                 select ce;

                    if (catequip2_.Count() == 0)
                    {
                        throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, idCatEquipamento2.ToString());
                    }

                    //Verifica disponibilidade do equipamento 1
                    var equip2_ = from Equipamento e in context.Equipamentos
                                  where e.CategoriaEquipamento.Id == idCatEquipamento1 && e.Disponivel
                                  select e;

                    if (equip2_.Count() == 0)
                    {
                        throw new EntidadesException(EntityExcCode.EQUIPAMENTOINDISPONIVEL, catequip2_.First().Nome);
                    }

                    if (local_.First().RestricoesCategoriaEquipamento.Contains(catequip2_.First()))
                    {
                        throw new EntidadesException(EntityExcCode.RESTRICAOLOCALEQUIPAMENTO, catequip2_.First().Nome);
                    }
                }

                //Verifica disponibilidade do local
                var reserva_ = from Reserva r in context.Reservas
                               where r.Data.Equals(data) && r.Turno.Equals(turno) && r.Horario.Equals(horario)
                                 && r.Local.Id == idLocal
                               select r.Local.Id;

                if (reserva_.Count() > 0)
                {
                    throw new EntidadesException(EntityExcCode.LOCALINDISPONIVEL, "");
                }

                //Realiza a reserva
                Reserva reserva = new Reserva();
                reserva.Data = data;
                reserva.Turno = turno;
                reserva.Horario = horario;
                reserva.Obs = obs;
                reserva.Local = local_.First();
                if (idCatEquipamento1 > 0)
                    reserva.CategoriasEquipamentos.Add(catequip1_.First());
                if (idCatEquipamento2 > 0)
                    reserva.CategoriasEquipamentos.Add(catequip2_.First());

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Locais disponíveis para reserva.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="horario"></param>
        /// <param name="turno"></param>
        /// <param name="reservaveis">Somente os reserváveis (true) ou todos (false)</param>
        /// <returns></returns>
        public static List<Local> ConsultarLocaisDisponiveis(DateTime data, string horario, string turno, bool somenteReservaveis)
        {
            List<Local> Locais = new List<Local>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                if (somenteReservaveis)
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
                    //Inclui não reserváveis mesmo que estejam em algum reserva 
                    var local2_ = from Local l in context.Locais
                                  where l.Disponivel && (!l.Reservavel || (l.Reservavel &&
                                  !(from Reserva r in context.Reservas
                                    where r.Data.Equals(data) && r.Turno.Equals(turno) && r.Horario.Equals(horario)
                                    select r.Local.Id).Contains(l.Id)))
                                  select l;

                    Locais = local2_.ToList();
                }
            }

            return Locais;
        }

        public static Reserva ConsultarReservaPorId(int id)
        {
            Reserva reserva = new Reserva();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var reservas_ = from Reserva r in context.Reservas.Include("Local")
                                .Include("CategoriasEquipamentos").Include("Usuario")
                                where r.Id == id
                                select r;

                if (reservas_.Count() == 0)
                {
                    throw new EntidadesException(EntityExcCode.RESERVAINEXISTENTE, id.ToString());
                }

                reserva = reservas_.First();
            }

            return reserva;
        }

        public static List<Reserva> ConsultarReservas()
        {
            List<Reserva> Reservas = new List<Reserva>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var reservas_ = from Reserva r in context.Reservas.Include("Local")
                                .Include("CategoriasEquipamentos").Include("Usuario")
                                where r.Data > DateTime.Today.AddDays(-1)
                                select r;

                Reservas = reservas_.ToList();
            }

            return Reservas;
        }

        public static void Remover(int id)
        {
            Reserva reserva = new Reserva();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var reservas_ = from Reserva r in context.Reservas
                                where r.Id == id
                                select r;

                if (reservas_.Count() == 0)
                {
                    throw new EntidadesException(EntityExcCode.RESERVAINEXISTENTE, id.ToString());
                }

                context.Reservas.Remove(reservas_.First());
                context.SaveChanges();
            }
        }
    }
}