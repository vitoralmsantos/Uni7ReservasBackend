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

        public static void Remover(int id)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var equipamento_ = from Equipamento e in context.Equipamentos
                                   where e.Id == id
                                   select e;

                if (equipamento_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.EQUIPAMENTOINEXISTENTE, id.ToString());



                int reservas = categoria_.First().Reservas.Count();
                if (reservas > 0)
                {
                    throw new EntidadesException(EntityExcCode.CATEGORIAPOSSUIRESERVAS, reservas.ToString());
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