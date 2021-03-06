﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using Uni7ReservasBackend.Controllers.Response;
using Uni7ReservasBackend.Controllers.TransferObjects;
using Uni7ReservasBackend.Models;
using Uni7ReservasBackend.Models.Entidades;

namespace Uni7ReservasBackend.Controllers
{
    [RoutePrefix("api/reserva")]
    public class ReservaController : ApiController
    {
        // GET api/<controller>
        [Route("")]
        public IHttpActionResult Get()
        {
            EntidadesResponse<ReservaTO> response = new EntidadesResponse<ReservaTO>();

            try
            {
                List<Reserva> reservas = Reserva.Consultar();

                foreach (Reserva r in reservas)
                {
                    ReservaTO rTO = new ReservaTO();
                    rTO.Id = r.Id;
                    rTO.Data = r.Data.ToString("dd/MM/yyyy");
                    rTO.DiaSemana = 
                    rTO.Horario = r.Horario;
                    rTO.Turno = r.Turno;
                    rTO.Obs = r.Obs;
                    rTO.ReservadoEm = r.ReservadoEm.ToString("dd/MM/yyyy HH:mm");
                    rTO.IdLocal = r.Local.Id;
                    rTO.NomeLocal = r.Local.Nome;
                    rTO.IdUsuario = r.Usuario.Id;
                    rTO.NomeUsuario = r.Usuario.Nome;
                    rTO.EmailUsuario = r.Usuario.Email;
                    rTO.ComentarioUsuario = r.ComentarioUsuario;
                    rTO.Satisfacao = r.Satisfacao.HasValue ? r.Satisfacao.Value : 0;
                    rTO.Equipamentos = new List<string>();
                    rTO.IdEquipamentos = new List<int>();
                    foreach (CategoriaEquipamento ce in r.CategoriasEquipamentos)
                    {
                        rTO.IdEquipamentos.Add(ce.Id);
                        rTO.Equipamentos.Add(ce.Nome);
                    }

                    response.Elementos.Add(rTO);
                }
            }
            catch (EntidadesException eex)
            {
                response.Status = (int)eex.Codigo;
                response.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Detalhes = ex.Message;
            }
            return Ok(response);
        }

        [Route("usuario/{idUsuario:int}")]
        public IHttpActionResult GetPorUsuario(int idUsuario)
        {
            EntidadesResponse<ReservaTO> response = new EntidadesResponse<ReservaTO>();
            try
            {
                List<Reserva> reservas = Reserva.ConsultarPorUsuario(idUsuario);

                foreach (Reserva r in reservas)
                {
                    ReservaTO rTO = new ReservaTO();
                    rTO.Id = r.Id;
                    rTO.Data = r.Data.ToString("dd/MM/yyyy");
                    rTO.Horario = r.Horario;
                    rTO.Turno = r.Turno;
                    rTO.Obs = r.Obs;
                    rTO.ReservadoEm = r.ReservadoEm.ToString("dd/MM/yyyy HH:mm");
                    rTO.IdLocal = r.Local.Id;
                    rTO.NomeLocal = r.Local.Nome;
                    rTO.IdUsuario = r.Usuario.Id;
                    rTO.NomeUsuario = r.Usuario.Nome;
                    rTO.EmailUsuario = r.Usuario.Email;
                    rTO.ComentarioUsuario = r.ComentarioUsuario;
                    rTO.Satisfacao = r.Satisfacao.HasValue ? r.Satisfacao.Value : 0;
                    rTO.ExibeAvaliacao = DateTime.Now > r.Data;
                    rTO.Equipamentos = new List<string>();
                    rTO.IdEquipamentos = new List<int>();
                    foreach (CategoriaEquipamento ce in r.CategoriasEquipamentos)
                    {
                        rTO.IdEquipamentos.Add(ce.Id);
                        rTO.Equipamentos.Add(ce.Nome);
                    }

                    response.Elementos.Add(rTO);
                }
            }
            catch (EntidadesException eex)
            {
                response.Status = (int)eex.Codigo;
                response.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Detalhes = ex.Message;
            }
            return Ok(response);
        }

        [Route("filtro")]
        public IHttpActionResult GetPorFiltro([FromUri]string dataDe, [FromUri]string dataAte, [FromUri]int tipo,
            [FromUri]int idLocal, [FromUri]int idCategoria, [FromUri]string obs)
        {
            EntidadesResponse<ReservaTO> response = new EntidadesResponse<ReservaTO>();

            try
            {
                DateTime? dateTimeDe=null, dateTimeAte=null;
                if (dataDe != null && dataDe.Length > 0)
                    dateTimeDe = DateTime.ParseExact(dataDe, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (dataAte != null && dataAte.Length > 0)
                    dateTimeAte = DateTime.ParseExact(dataAte, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                List<Reserva> reservas = Reserva.ConsultarPorFiltro(dateTimeDe, dateTimeAte, tipo, idLocal, idCategoria, obs);

                foreach (Reserva r in reservas)
                {
                    ReservaTO rTO = new ReservaTO();
                    rTO.Id = r.Id;
                    rTO.Data = r.Data.ToString("dd/MM/yyyy");
                    rTO.Horario = r.Horario;
                    rTO.Turno = r.Turno;
                    rTO.Obs = r.Obs;
                    rTO.ReservadoEm = r.ReservadoEm.ToString("dd/MM/yyyy HH:mm");
                    rTO.IdLocal = r.Local.Id;
                    rTO.NomeLocal = r.Local.Nome;
                    rTO.IdUsuario = r.Usuario.Id;
                    rTO.NomeUsuario = r.Usuario.Nome;
                    rTO.EmailUsuario = r.Usuario.Email;
                    rTO.ComentarioUsuario = r.ComentarioUsuario;
                    rTO.Satisfacao = r.Satisfacao.HasValue ? r.Satisfacao.Value : 0;
                    rTO.ExibeAvaliacao = DateTime.Now > r.Data;
                    rTO.Equipamentos = new List<string>();
                    rTO.IdEquipamentos = new List<int>();
                    foreach (CategoriaEquipamento ce in r.CategoriasEquipamentos)
                    {
                        rTO.IdEquipamentos.Add(ce.Id);
                        rTO.Equipamentos.Add(ce.Nome);
                    }

                    response.Elementos.Add(rTO);
                }
            }
            catch (EntidadesException eex)
            {
                response.Status = (int)eex.Codigo;
                response.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Detalhes = ex.Message;
            }
            return Ok(response);
        }

        // GET api/<controller>/5
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            EntidadeResponse<ReservaTO> response = new EntidadeResponse<ReservaTO>();
            response.Elemento = new ReservaTO();

            try
            {
                Reserva r = Reserva.ConsultarPorId(id);

                response.Elemento.Data = r.Data.ToString("ddMMyyyy");
                response.Elemento.Horario = r.Horario;
                response.Elemento.Turno = r.Turno;
                response.Elemento.Obs = r.Obs;
                response.Elemento.ReservadoEm = r.ReservadoEm.ToString("ddMMyyyy HHmm");
                response.Elemento.NomeLocal = r.Local.Nome;
                response.Elemento.NomeUsuario = r.Usuario.Nome;
                response.Elemento.EmailUsuario = r.Usuario.Email;
                response.Elemento.ComentarioUsuario = r.ComentarioUsuario;
                response.Elemento.Satisfacao = r.Satisfacao.HasValue ? r.Satisfacao.Value : 0;
                response.Elemento.ExibeAvaliacao = DateTime.Now > r.Data;
                foreach (CategoriaEquipamento ce in r.CategoriasEquipamentos)
                {
                    response.Elemento.Equipamentos.Add(ce.Nome);
                }
            }
            catch (EntidadesException eex)
            {
                response.Status = (int)eex.Codigo;
                response.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Detalhes = ex.Message;
            }
            return Ok(response);
        }

        // POST api/<controller>
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]ReservaRegistroTO reserva)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                DateTime data = DateTime.ParseExact(reserva.Data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Reserva.Reservar(reserva.IdUsuario, data, reserva.Horario, reserva.Turno, reserva.IdLocal, 
                    reserva.Obs, reserva.IdCategoria);
            }
            catch (EntidadesException eex)
            {
                response.Status = (int)eex.Codigo;
                response.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Detalhes = ex.Message;
            }
            return Ok(response);
        }

        [Route("periodo")]
        [HttpPost]
        public IHttpActionResult ReservarPeriodo([FromBody]ReservaRegistroTO reserva)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                DateTime dataDe = DateTime.ParseExact(reserva.Data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dataAte = DateTime.ParseExact(reserva.DataAte, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Reserva.ReservarPeriodo(reserva.IdUsuario, dataDe, dataAte, reserva.Horario, reserva.Turno, 
                    reserva.IdLocal, reserva.Obs, reserva.IdCategoria);
            }
            catch (EntidadesException eex)
            {
                response.Status = (int)eex.Codigo;
                response.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Detalhes = ex.Message;
            }
            return Ok(response);
        }

        // POST api/<controller>
        [Route("obs")]
        public IHttpActionResult AtualizarObs([FromBody]ReservaRegistroTO reserva)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                Reserva.AtualizarObs(reserva.Id, reserva.Obs);
            }
            catch (EntidadesException eex)
            {
                response.Status = (int)eex.Codigo;
                response.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Detalhes = ex.Message;
            }
            return Ok(response);
        }

        // POST api/<controller>
        [Route("avaliacao")]
        public IHttpActionResult AtualizarAvaliacao([FromBody]ReservaRegistroTO reserva)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                Reserva.AtualizarAvaliacao(reserva.Id, reserva.Satisfacao, reserva.ComentarioUsuario);
            }
            catch (EntidadesException eex)
            {
                response.Status = (int)eex.Codigo;
                response.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Detalhes = ex.Message;
            }
            return Ok(response);
        }

        // DELETE api/<controller>/5
        [Route("remover")]
        [HttpPost]
        public IHttpActionResult Remover([FromBody]ReservaRegistroTO reserva)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                Reserva.Remover(reserva.Id);
            }
            catch (EntidadesException eex)
            {
                response.Status = (int)eex.Codigo;
                response.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Detalhes = ex.Message;
            }
            return Ok(response);
        }
    }
}