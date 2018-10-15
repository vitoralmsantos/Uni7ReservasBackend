using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Uni7ReservasBackend.Models.Entidades;

namespace Uni7ReservasBackend.Models
{
    public partial class Local
    {
        public static List<Local> Consultar()
        {
            List<Local> Locais = new List<Local>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var local_ = from Local l in context.Locais
                                   select l;

                Locais = local_.ToList();
            }

            return Locais;
        }

        public static List<Local> ConsultarReservaveis(bool disponivel)
        {
            List<Local> Locais = new List<Local>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var equipamento_ = from Local l in context.Locais
                                   where l.Reservavel && l.Disponivel == disponivel
                                   select l;

                Locais = equipamento_.ToList();
            }

            return Locais;
        }

        public static Local ConsultarPorId(int id)
        {
            Local local = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var local_ = from Local l in context.Locais
                                   where l.Id == id
                                   select l;

                if (local_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.LOCALINEXISTENTE, id.ToString());

                local = local_.First();
            }

            return local;
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

        public static int Cadastrar(string nome, bool reservavel, bool disponivel, TIPOLOCAL tipo)
        {
            if (nome == null || nome.Length == 0)
                throw new EntidadesException(EntityExcCode.NOMELOCALVAZIO, "");

            Local local = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                local = new Local();
                local.Nome = nome;
                local.Reservavel = reservavel;
                local.Disponivel = disponivel;
                local.Tipo = tipo;

                context.Locais.Add(local);
                context.SaveChanges();
            }

            return local == null ? 0 : local.Id;
        }

        public static void Atualizar(int id, string nome, bool reservavel, bool disponivel, TIPOLOCAL tipo)
        {
            if (nome == null || nome.Length == 0)
                throw new EntidadesException(EntityExcCode.NOMELOCALVAZIO, "");

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var local_ = from Local l in context.Locais
                                 where l.Id == id
                                 select l;

                if (local_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.LOCALINEXISTENTE, id.ToString());

                Local local = local_.First();
                local.Nome = nome;
                local.Reservavel = reservavel;
                local.Disponivel = disponivel;
                local.Tipo = tipo;

                context.SaveChanges();
            }
        }

        public static void Remover(int id)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var local_ = from Local l in context.Locais.Include("Reservas").Include("Locais")
                                 where l.Id == id
                                 select l;

                if (local_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.LOCALINEXISTENTE, id.ToString());

                if (local_.First().Reservas.Count > 0)
                {
                    string info = "";
                    foreach (var r in local_.First().Reservas)
                    {
                        info += String.Format("[{0} {1} {2}] ", r.Data.ToShortDateString(),
                            r.Turno, r.Horario);
                    }
                    throw new EntidadesException(EntityExcCode.LOCALPOSSUIRESERVAS, info);
                }
                    

                context.Locais.Remove(local_.First());
                context.SaveChanges();
            }
        }
    }
}