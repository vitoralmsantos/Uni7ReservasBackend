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
    public partial class CategoriaEquipamento
    {
        public static List<CategoriaEquipamento> ConsultarCategorias()
        {
            List<CategoriaEquipamento> Categorias = new List<CategoriaEquipamento>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var categorias_ = from CategoriaEquipamento ce in context.Categorias
                                  select ce;

                Categorias = categorias_.ToList();
            }

            return Categorias;
        }

        public static CategoriaEquipamento ConsultarCategoriaPorId(int idCategoria)
        {
            CategoriaEquipamento categoria = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var categoria_ = from CategoriaEquipamento ce in context.Categorias
                                 where ce.Id == idCategoria
                                 select ce;

                if (categoria_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, idCategoria.ToString());

                categoria = categoria_.First();
            }

            return categoria;
        }

        public static int Cadastrar(string nome)
        {
            CategoriaEquipamento categoria = null;
            if (nome == null || nome.Length == 0)
                throw new EntidadesException(EntityExcCode.NOMECATEGORIAVAZIO, "");

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var categoria_ = from CategoriaEquipamento ce in context.Categorias
                                 where ce.Nome == nome
                                 select ce;

                if (categoria_.Count() > 0)
                    throw new EntidadesException(EntityExcCode.CATEGORIAJACADASTRADA, nome);

                categoria = new CategoriaEquipamento();
                categoria.Nome = nome;

                context.Categorias.Add(categoria);
                context.SaveChanges();
            }

            return categoria.Id;
        }

        public static void Atualizar(int idCategoria, string nome)
        {
            if (nome == null || nome.Length == 0)
                throw new EntidadesException(EntityExcCode.NOMECATEGORIAVAZIO, "");

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var categoria_ = from CategoriaEquipamento ce in context.Categorias
                                 where ce.Id == idCategoria
                                 select ce;

                if (categoria_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, idCategoria.ToString());

                CategoriaEquipamento categoria = categoria_.First();
                categoria.Nome = nome;

                context.SaveChanges();
            }
        }

        public static void Remover(int idCategoria)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var categoria_ = from CategoriaEquipamento ce in context.Categorias.Include("Reservas").Include("Equipamentos")
                               where ce.Id == idCategoria
                               select ce;

                if (categoria_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, idCategoria.ToString());

                if (categoria_.First().Reservas.Count > 0)
                {
                    string info = "";
                    foreach (var r in categoria_.First().Reservas)
                    {
                        info += String.Format("[{0} {1} {2}] ", r.Data.ToShortDateString(),
                            r.Turno, r.Horario);
                    }
                    throw new EntidadesException(EntityExcCode.CATEGORIAPOSSUIRESERVAS, info);
                }

                int equips = categoria_.First().Equipamentos.Count();
                if (equips > 0)
                {
                    throw new EntidadesException(EntityExcCode.CATEGORIAPOSSUIEQUIPAMENTOS, equips.ToString());
                }

                context.Categorias.Remove(categoria_.First());
                context.SaveChanges();
            }
        }
    }
}