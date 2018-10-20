using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Uni7ReservasBackend.Controllers.Response;
using Uni7ReservasBackend.Controllers.TransferObjects;
using Uni7ReservasBackend.Models;
using Uni7ReservasBackend.Models.Entidades;

namespace Uni7ReservasBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
                    rTO.NomeLocal = r.Local.Nome;
                    rTO.NomeUsuario = r.Usuario.Nome;
                    rTO.EmailUsuario = r.Usuario.Email;
                    rTO.Equipamentos = new List<string>();
                    foreach (CategoriaEquipamento ce in r.CategoriasEquipamentos)
                    {
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
                    rTO.Data = r.Data.DayOfWeek.ToString() + " " + r.Data.ToString("dd/MM/yyyy");
                    rTO.Horario = r.Horario;
                    rTO.Turno = r.Turno;
                    rTO.Obs = r.Obs;
                    rTO.ReservadoEm = r.ReservadoEm.ToString("dd/MM/yyyy HH:mm");
                    rTO.NomeLocal = r.Local.Nome;
                    rTO.NomeUsuario = r.Usuario.Nome;
                    rTO.EmailUsuario = r.Usuario.Email;
                    rTO.Equipamentos = new List<string>();
                    foreach (CategoriaEquipamento ce in r.CategoriasEquipamentos)
                    {
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
        public IHttpActionResult Post([FromBody]ReservaRegistroTO reserva)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                DateTime data = DateTime.ParseExact(reserva.Data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Reserva.Reservar(reserva.IdUsuario, data, reserva.Horario, reserva.Turno, reserva.IdLocal, reserva.Obs, reserva.IdCategoria);
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

        // PUT api/<controller>/5
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody]string categoria)
        {

            return Ok();
        }

        // DELETE api/<controller>/5
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                Reserva.Remover(id);
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