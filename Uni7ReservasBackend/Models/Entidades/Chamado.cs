using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uni7ReservasBackend.Models.Entidades;

namespace Uni7ReservasBackend.Models
{
    public partial class Chamado
    {
        public static List<Chamado> Consultar()
        {
            List<Chamado> chamados = new List<Chamado>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var chamado_ = from Chamado l in context.Chamados
                             select l;

                chamados = chamado_.ToList();
            }

            return chamados;
        }

        public static List<Chamado> ConsultarPorStatus(STATUSCHAMADO status)
        {
            List<Chamado> chamados = new List<Chamado>();

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var chamado_ = from Chamado c in context.Chamados
                                   where c.Status == status
                                   select c;

                chamados = chamado_.ToList();
            }

            return chamados;
        }

        public static Chamado ConsultarPorId(int id)
        {
            Chamado chamado = null;
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var chamado_ = from Chamado l in context.Chamados
                             where l.Id == id
                             select l;

                if (chamado_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.LOCALINEXISTENTE, id.ToString());

                chamado = chamado_.First();
            }

            return chamado;
        }

        public static int Cadastrar(string descricao, DateTime dataLimite, string observacoes, string telefone)
        {
            if (descricao == null || descricao.Length == 0)
                throw new EntidadesException(EntityExcCode.DESCRICAOCHAMADOVAZIO, "");

            Chamado chamado = null;

            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                chamado = new Chamado();
                chamado.Descricao = descricao;
                chamado.DataLimite = dataLimite;
                chamado.Observacoes = observacoes;
                chamado.Status = STATUSCHAMADO.ABERTO;
                chamado.Telefone = telefone;

                context.Chamados.Add(chamado);
                context.SaveChanges();
            }

            return chamado == null ? 0 : chamado.Id;
        }

        /// <summary>
        /// Essa atualização será realizada somente pelo administrador.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="observacoes"></param>
        /// <param name="status"></param>
        /// <param name="dataPrevista"></param>
        public static void Atualizar(int id, string observacoes, STATUSCHAMADO status, DateTime dataPrevista)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var chamado_ = from Chamado c in context.Chamados
                             where c.Id == id
                             select c;

                if (chamado_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.CHAMADOINEXISTENTE, id.ToString());

                Chamado chamado = chamado_.First();
                chamado.Observacoes = observacoes;
                chamado.Status = status;
                chamado.DataPrevista = dataPrevista;

                context.SaveChanges();
            }
        }

        public static void Remover(int id)
        {
            using (Uni7ReservasEntities context = new Uni7ReservasEntities())
            {
                var chamado_ = from Chamado c in context.Chamados
                             where c.Id == id
                             select c;

                if (chamado_.Count() == 0)
                    throw new EntidadesException(EntityExcCode.CHAMADOINEXISTENTE, id.ToString());

                context.Chamados.Remove(chamado_.First());
                context.SaveChanges();
            }
        }
    }
}