using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uni7ReservasBackend.Models.Entidades;

namespace Uni7ReservasBackend.Models
{
    public partial class Reserva
    {
        public static void Reservar(int idUsuario, DateTime data, string horario, string turno, int idLocal,
            string obs, int idCatEquipamento)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                //Consulta usuário
                var usuario_ = from Usuario u in context.Usuarios
                               where u.Id == idUsuario
                               select u;

                if (usuario_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.IDUSUARIONAOCADASTRADO, idUsuario.ToString());

                //Consulta local
                var local_ = from Local l in context.Locais.Include("RestricoesCategoriaEquipamento")
                             where l.Id == idLocal
                             select l;

                if (local_.Count() == 0)
                {
                    throw new EntidadesException(EntityExcCode.LOCALINEXISTENTE, idLocal.ToString());
                }
                else if (!local_.First().Reservavel && idCatEquipamento == 0)
                {
                    throw new EntidadesException(EntityExcCode.EQUIPAMENTONECESSARIO, "");
                }

                IEnumerable<CategoriaEquipamento> catequip1_ = null;
                if (idCatEquipamento > 0)
                {
                    //Consulta categoria do equipamento 1
                    catequip1_ = from CategoriaEquipamento ce in context.Categorias
                                 where ce.Id == idCatEquipamento
                                 select ce;

                    if (catequip1_.Count() == 0)
                    {
                        throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, idCatEquipamento.ToString());
                    }

                    //Verifica disponibilidade do equipamento 1
                    var equip1_ = from Equipamento e in context.Equipamentos
                                  where e.CategoriaEquipamento.Id == idCatEquipamento && e.Disponivel
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

                //Verifica disponibilidade do local
                var reserva_ = from Reserva r in context.Reservas
                               where r.Data.Equals(data) && r.Turno.Equals(turno) && r.Horario.Equals(horario)
                                 && r.Local.Id == idLocal
                               select new { localId = r.Local.Id, usuarioId = r.Usuario.Id, r.Usuario.Nome };

                if (reserva_.Count() > 0)
                {
                    if (reserva_.First().usuarioId == idUsuario)
                        throw new EntidadesException(EntityExcCode.LOCALINDISPONIVELPROPRIOUSUARIO, "");
                    else
                        throw new EntidadesException(EntityExcCode.LOCALINDISPONIVEL, reserva_.First().Nome);
                }

                //Realiza a reserva
                Reserva reserva = new Reserva();
                reserva.Data = data;
                reserva.ReservadoEm = DateTime.Now;
                reserva.Turno = turno;
                reserva.Horario = horario;
                reserva.Obs = obs;
                reserva.Usuario = usuario_.First();
                reserva.Local = local_.First();
                if (idCatEquipamento > 0)
                    reserva.CategoriasEquipamentos.Add(catequip1_.First());

                context.Reservas.Add(reserva);
                context.SaveChanges();
            }
        }

        public static Reserva AtualizarObs(int id, string obs)
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

                reservas_.First().Obs = obs;
                context.SaveChanges();
            }

            return reserva;
        }

        public static Reserva ConsultarPorId(int id)
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

        public static List<Reserva> Consultar()
        {
            List<Reserva> Reservas = new List<Reserva>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                DateTime ontem = DateTime.Today.AddDays(-1);
                var reservas_ = from Reserva r in context.Reservas.Include("Local")
                                .Include("CategoriasEquipamentos").Include("Usuario")
                                where r.Data > ontem
                                select r;

                Reservas = reservas_.ToList();
            }

            return Reservas;
        }

        public static List<Reserva> ConsultarPorUsuario(int idUsuario)
        {
            List<Reserva> Reservas = new List<Reserva>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                DateTime ontem = DateTime.Today.AddDays(-1);
                var reservas_ = from Reserva r in context.Reservas.Include("Local")
                                .Include("CategoriasEquipamentos").Include("Usuario")
                                where r.Usuario.Id == idUsuario && r.Data > ontem
                                select r;

                string[] customOrder = { "M", "T", "N" };
                Reservas = reservas_.ToList()
                    .OrderBy(res => res.Data)
                    .ThenBy(res => Array.IndexOf(customOrder, res.Turno))
                    .ThenBy(res => res.Horario).ToList();
            }

            return Reservas;
        }

        public static void Remover(int id)
        {
            Reserva reserva = new Reserva();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var reservas_ = from Reserva r in context.Reservas.Include("CategoriasEquipamentos")
                                where r.Id == id
                                select r;

                if (reservas_.Count() == 0)
                {
                    throw new EntidadesException(EntityExcCode.RESERVAINEXISTENTE, id.ToString());
                }

                reservas_.First().CategoriasEquipamentos.Clear();
                context.SaveChanges();
                context.Reservas.Remove(reservas_.First());
                context.SaveChanges();
            }
        }
    }
}