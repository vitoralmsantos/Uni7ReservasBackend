﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uni7ReservasBackend.Models.Entidades;
using static Uni7ReservasBackend.Models.Entidades.Util;

namespace Uni7ReservasBackend.Models
{
    public partial class Reserva
    {
        public static void Reservar(int idUsuario, DateTime data, string horario, string turno, int idLocal,
            string obs, int idCatEquipamento)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                //Verifica data
                DateTime limite = data.AddMinutes(Util.HorarioParaHora(horario, turno) - 60);
                if (DateTime.Now > limite)
                {
                    throw new EntidadesException(EntityExcCode.DATAPASSADA, data.ToShortDateString() + " " + turno + " " + horario);
                }

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

                IEnumerable<CategoriaEquipamento> catequip_ = null;
                if (idCatEquipamento > 0)
                {
                    //Consulta categoria do equipamento 1
                    catequip_ = from CategoriaEquipamento ce in context.Categorias.Include("Equipamentos")
                                where ce.Id == idCatEquipamento
                                select ce;

                    if (catequip_.Count() == 0)
                    {
                        throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, idCatEquipamento.ToString());
                    }

                    CategoriaEquipamento catequip = catequip_.First();

                    //Verifica restrições do equipamento
                    if (local_.First().RestricoesCategoriaEquipamento.Contains(catequip_.First()))
                    {
                        throw new EntidadesException(EntityExcCode.RESTRICAOLOCALEQUIPAMENTO, catequip.Nome);
                    }

                    //Reservas daquela categoria
                    var reservaequip_ = from Reserva r in context.Reservas
                                        where r.Data.Equals(data) && r.Turno.Equals(turno) && r.Horario.Equals(horario)
                                        && ((from CategoriaEquipamento cer in r.CategoriasEquipamentos
                                             where cer.Id == idCatEquipamento
                                             select cer).Count() > 0)
                                        select r;

                    if (reservaequip_.Count() >= catequip.Equipamentos.Where(e => e.Disponivel).Count())
                    {
                        throw new EntidadesException(EntityExcCode.EQUIPAMENTOINDISPONIVEL, "");
                    }
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
                    reserva.CategoriasEquipamentos.Add(catequip_.First());

                context.Reservas.Add(reserva);
                context.SaveChanges();
            }
        }

        public static void ReservarPeriodo(int idUsuario, DateTime dataDe, DateTime dataAte, string horario,
            string turno, int idLocal, string obs, int idCatEquipamento)
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

                IEnumerable<CategoriaEquipamento> catequip_ = null;
                if (idCatEquipamento > 0)
                {
                    //Consulta categoria do equipamento
                    catequip_ = from CategoriaEquipamento ce in context.Categorias
                                where ce.Id == idCatEquipamento
                                select ce;

                    if (catequip_.Count() == 0)
                    {
                        throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, idCatEquipamento.ToString());
                    }

                    if (local_.First().RestricoesCategoriaEquipamento.Contains(catequip_.First()))
                    {
                        throw new EntidadesException(EntityExcCode.RESTRICAOLOCALEQUIPAMENTO, catequip_.First().Nome);
                    }
                }

                DateTime data = dataDe;

                if (dataDe > dataAte)
                {
                    throw new EntidadesException(EntityExcCode.DATASINVALIDAS, "");
                }

                List<Reserva> reservas = new List<Reserva>();
                while (data <= dataAte)
                {
                    //Verifica disponibilidade do local
                    var reservalocal_ = from Reserva r in context.Reservas
                                        where r.Data.Equals(data) && r.Turno.Equals(turno) && r.Horario.Equals(horario)
                                          && r.Local.Id == idLocal
                                        select new { localId = r.Local.Id, usuarioId = r.Usuario.Id, r.Usuario.Nome };

                    if (reservalocal_.Count() > 0)
                    {
                        if (reservalocal_.First().usuarioId == idUsuario)
                            throw new EntidadesException(EntityExcCode.LOCALINDISPONIVELPROPRIOUSUARIO, data.ToString("ddMMyyyy"));
                        else
                            throw new EntidadesException(EntityExcCode.LOCALINDISPONIVEL, reservalocal_.First().Nome + " " + data.ToString("ddMMyyyy"));
                    }

                    //Verifica disponibilidade do equipamento
                    if (idCatEquipamento > 0)
                    {
                        var cat_ = from CategoriaEquipamento ce in context.Categorias.Include("Equipamentos")
                                   where ce.Id == idCatEquipamento
                                   select ce;

                        CategoriaEquipamento catequip = cat_.First();

                        //Reservas daquela categoria
                        var reservaequip_ = from Reserva r in context.Reservas
                                            where r.Data.Equals(data) && r.Turno.Equals(turno) && r.Horario.Equals(horario)
                                            && ((from CategoriaEquipamento cer in r.CategoriasEquipamentos
                                                 where cer.Id == idCatEquipamento
                                                 select cer).Count() > 0)
                                            select r;

                        if (reservaequip_.Count() >= catequip.Equipamentos.Where(e => e.Disponivel).Count())
                        {
                            throw new EntidadesException(EntityExcCode.EQUIPAMENTOINDISPONIVEL, data.ToString("ddMMyyyy"));
                        }
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
                        reserva.CategoriasEquipamentos.Add(catequip_.First());

                    reservas.Add(reserva);

                    //Passa para próximo dia
                    data.AddDays(7);
                }

                foreach (Reserva r in reservas)
                {
                    context.Reservas.Add(r);
                    context.SaveChanges();
                }
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

        public static Reserva AtualizarAvaliacao(int id, int satisfacao, string comentarioUsuario)
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

                reservas_.First().ComentarioUsuario = comentarioUsuario;
                reservas_.First().Satisfacao = satisfacao;
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
                                where r.Usuario.Id == idUsuario && (r.Data > ontem || r.Satisfacao == null)
                                select r;

                string[] customOrder = { "M", "T", "N" };
                Reservas = reservas_.ToList()
                    .OrderBy(res => res.Data)
                    .ThenBy(res => Array.IndexOf(customOrder, res.Turno))
                    .ThenBy(res => res.Horario).ToList();
            }

            return Reservas;
        }

        public static List<Reserva> ConsultarPorFiltro(DateTime? dataDe, DateTime? dataAte, int tipo, int idLocal, 
            int idCategoria, string obs)
        {
            List<Reserva> Reservas = new List<Reserva>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                if (!dataDe.HasValue) dataDe = DateTime.MinValue;
                if (!dataAte.HasValue) dataAte = DateTime.MaxValue;

                var reservas_ = from Reserva r in context.Reservas.Include("Local")
                                    .Include("CategoriasEquipamentos.Equipamentos").Include("Usuario")
                                where r.Data >= dataDe && r.Data <= dataAte
                                select r;

                string[] customOrder = { "M", "T", "N" };

                Reservas = reservas_.ToList()
                    .OrderBy(res => res.Data)
                    .ThenBy(res => Array.IndexOf(customOrder, res.Turno))
                    .ThenBy(res => res.Horario).ToList();

                //Somente laboratórios
                if (tipo == 1)
                {
                    Reservas = Reservas.Where(r => r.Local.Reservavel).ToList();
                }
                //Equipamentos
                else if (tipo == 2)
                {
                    Reservas = Reservas.Where(r => r.CategoriasEquipamentos.Count() > 0).ToList();
                }
                //Local e/ou equipamento específico
                else if (tipo == 3)
                {
                    if (idLocal > 0)
                    {
                        Reservas = Reservas.Where(r => r.Local.Id == idLocal).ToList();
                    }

                    if (idCategoria > 0)
                    {
                        Reservas = Reservas.Where(r => r.CategoriasEquipamentos.Where(c=>c.Id == idCategoria).Count()>0).ToList();
                    }
                }

                if (obs != null && obs.Length > 0)
                {
                    Reservas = Reservas.Where(r => r.Obs != null && r.Obs.Contains(obs)).ToList();
                }
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