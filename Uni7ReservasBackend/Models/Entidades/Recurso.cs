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
    public partial class Recurso
    {
        public static List<Recurso> Consultar()
        {
            List<Recurso> Recursos = new List<Recurso>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var recurso_ = from Recurso r in context.Recursos
                               select r;

                Recursos = recurso_.ToList();
            }

            return Recursos;
        }

        public static Recurso ConsultarPorId(int id)
        {
            Recurso recurso = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var recurso_ = from Recurso r in context.Recursos
                               where r.Id == id
                               select r;

                if (recurso_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.RECURSOINEXISTENTE, id.ToString());

                recurso = recurso_.First();
            }

            return recurso;
        }

        public static int Cadastrar(string nome, string detalhes, TIPORECURSO tipo)
        {
            Recurso recurso = null;

            if (nome == null || nome.Length == 0)
                throw new EntidadesException(EntityExcCode.NOMERECURSOVAZIO, "");

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                recurso = new Recurso();
                recurso.Nome = nome;
                recurso.Detalhes = detalhes;
                recurso.Tipo = tipo;

                context.Recursos.Add(recurso);
                context.SaveChanges();
            }

            return recurso.Id;
        }

        public static void Atualizar(int id, string nome, string detalhes, TIPORECURSO tipo)
        {
            if (nome == null || nome.Length == 0)
                throw new EntidadesException(EntityExcCode.NOMERECURSOVAZIO, "");

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var recurso_ = from Recurso r in context.Recursos
                               where r.Id == id
                               select r;

                if (recurso_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.RECURSOINEXISTENTE, id.ToString());

                Recurso recurso = recurso_.First();
                recurso.Nome = nome;
                recurso.Detalhes = detalhes;
                recurso.Tipo = tipo;

                context.SaveChanges();
            }
        }

        public static void CadastrarPorLocal(int idRecurso, int idLocal, int qtde)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var recurso_ = from Recurso r in context.Recursos
                               where r.Id == idRecurso
                               select r;

                if (recurso_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.RECURSOINEXISTENTE, idRecurso.ToString());

                var local_ = from Local l in context.Locais
                             where l.Id == idLocal
                             select l;

                if (local_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.LOCALINEXISTENTE, idLocal.ToString());

                RecursoLocal rl = new RecursoLocal();
                rl.Recurso = recurso_.First();
                rl.Local = local_.First();
                rl.Qtde = qtde;
                context.RecursosLocais.Add(rl);
                context.SaveChanges();
            }
        }

        public static void Remover(int id)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var recurso_ = from Recurso r in context.Recursos.Include("Locais.RecursoLocal")
                               where r.Id == id
                               select r;

                if (recurso_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.RECURSOINEXISTENTE, id.ToString());

                if (recurso_.First().Locais.Count > 0)
                {
                    string info = "";
                    foreach (var r in recurso_.First().Locais)
                    {
                        info += String.Format("[{0}] ", r.Local.Nome);
                    }
                    throw new EntidadesException(EntityExcCode.RECURSOPOSSUILOCAIS, info);
                }


                context.Recursos.Remove(recurso_.First());
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Remove o recurso do local específico.
        /// </summary>
        /// <param name="idRecurso"></param>
        /// <param name="idLocal"></param>
        public static void RemoverPorLocal(int idRecurso, int idLocal)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var recurso_ = from Recurso r in context.Recursos
                               where r.Id == idRecurso
                               select r;

                if (recurso_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.RECURSOINEXISTENTE, idRecurso.ToString());

                var local_ = from Local l in context.Locais
                             where l.Id == idLocal
                             select l;

                if (local_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.LOCALINEXISTENTE, idLocal.ToString());

                var recursolocal_ = from RecursoLocal rl in context.RecursosLocais
                                    where rl.Recurso.Id == idLocal && rl.Local.Id == idLocal
                                    select rl;

                if (recursolocal_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.RECURSOLOCALINEXISTENTE, "");

                context.RecursosLocais.Remove(recursolocal_.First());
                context.SaveChanges();
            }
        }
    }
}