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
    public partial class Equipamento
    {
        public static List<Equipamento> Consultar()
        {
            List<Equipamento> Equipamentos = new List<Equipamento>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var equipamento_ = from Equipamento e in context.Equipamentos
                                   select e;

                Equipamentos = equipamento_.ToList();
            }

            return Equipamentos;
        }

        public static Equipamento ConsultarPorId(int id)
        {
            Equipamento equipamento = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var equipamento_ = from Equipamento e in context.Equipamentos
                                 where e.Id == id
                                 select e;

                if (equipamento_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.EQUIPAMENTOINEXISTENTE, id.ToString());
                else
                    equipamento = equipamento_.First();
            }

            return equipamento;
        }

        public static void Cadastrar(string modelo, string serie, bool disponivel, int idCategoria)
        {
            if (modelo == null || modelo.Length == 0)
                throw new EntidadesException(EntityExcCode.MODELOEQUIPAMENTOVAZIO, "");

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var categoria_ = from CategoriaEquipamento ce in context.Categorias
                                 where ce.Id == idCategoria
                                 select ce;

                if (categoria_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, idCategoria.ToString());

                Equipamento equipamento = new Equipamento();
                equipamento.Modelo = modelo;
                equipamento.Serie = serie;
                equipamento.Disponivel = disponivel;
                equipamento.CategoriaEquipamento = categoria_.First();

                context.Equipamentos.Add(equipamento);
                context.SaveChanges();
            }
        }

        public static void Atualizar(int id, string modelo, string serie, bool disponivel, int idCategoria)
        {
            if (modelo == null || modelo.Length == 0)
                throw new EntidadesException(EntityExcCode.MODELOEQUIPAMENTOVAZIO, "");

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var equipamento_ = from Equipamento e in context.Equipamentos
                                 where e.Id == id
                                 select e;

                if (equipamento_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.EQUIPAMENTOINEXISTENTE, id.ToString());

                var categoria_ = from CategoriaEquipamento ce in context.Categorias
                                 where ce.Id == idCategoria
                                 select ce;

                if (categoria_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, idCategoria.ToString());

                Equipamento equipamento = equipamento_.First();
                equipamento.Modelo = modelo;
                equipamento.Serie = serie;
                equipamento.Disponivel = disponivel;
                equipamento.CategoriaEquipamento = categoria_.First();

                context.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="verificaQtde">Verifica se existe algum dia/turno/horário com o limite de reservas daquele equipamento.</param>
        public static void Remover(int id, bool verificaQtde)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var equipamento_ = from Equipamento e in context.Equipamentos
                                   where e.Id == id
                                   select e;

                if (equipamento_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.EQUIPAMENTOINEXISTENTE, id.ToString());

                if (verificaQtde)
                {
                    var categoria_ = from CategoriaEquipamento ce in context.Categorias.Include("Equipamentos")
                                     where ce.Id == id
                                     select ce;

                    if (categoria_.Count() == 0)
                        throw new EntidadesException(EntityExcCode.CATEGORIAINEXISTENTE, id.ToString());

                    CategoriaEquipamento categoria = categoria_.First();
                    int qtdeCategoria = categoria_.First().Equipamentos.Where(e => e.Disponivel).Count();

                    var reservas_ = from Reserva r in context.Reservas
                                    where r.CategoriasEquipamentos.Contains(categoria) && r.Data > DateTime.Today.AddDays(-1)
                                    group r by new { r.Data, r.Turno, r.Horario } into g
                                    where g.Count() >= qtdeCategoria
                                    select new { Reserva = g.Key, Qtde = g.Count() };

                    if (reservas_.Count() > 0)
                    {
                        string info = "";
                        foreach (var r in reservas_)
                        {
                            info += String.Format("[{0} {1} {2}] ", r.Reserva.Data.ToShortDateString(),
                                r.Reserva.Turno, r.Reserva.Horario);
                        }
                        throw new EntidadesException(EntityExcCode.EQUIPAMENTONOLIMITEDERESERVAS, info);
                    }
                }

                context.Equipamentos.Remove(equipamento_.First());
                context.SaveChanges();
            }
        }
    }
}