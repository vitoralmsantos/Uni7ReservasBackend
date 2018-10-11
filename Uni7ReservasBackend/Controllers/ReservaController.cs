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
            ReservasResponse rResponse = new ReservasResponse();
            rResponse.Reservas = new List<ReservaTO>();

            try
            {
                List<Reserva> reservas = Reserva.ConsultarReservas();

                foreach (Reserva r in reservas)
                {
                    ReservaTO rTO = new ReservaTO();
                    rTO.Data = r.Data.ToString("ddMMyyyy");
                    rTO.Horario = r.Horario;
                    rTO.Turno = r.Turno;
                    rTO.Obs = r.Obs;
                    rTO.ReservadoEm = r.ReservadoEm.ToString("ddMMyyyy HHmm");
                    rTO.NomeLocal = r.Local.Nome;
                    rTO.NomeUsuario = r.Usuario.Nome;
                    rTO.EmailUsuario = r.Usuario.Email;
                    foreach (CategoriaEquipamento ce in r.CategoriasEquipamentos)
                    {
                        rTO.Equipamentos.Add(ce.Nome);
                    }

                    rResponse.Reservas.Add(rTO);
                }
            }
            catch (EntidadesException eex)
            {
                rResponse.Status = (int)eex.Codigo;
                rResponse.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                rResponse.Status = -1;
                rResponse.Detalhes = ex.Message;
            }
            return Ok(rResponse);
        }

        // GET api/<controller>/5
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            ReservaResponse rResponse = new ReservaResponse();
            rResponse.Reserva = new ReservaTO();

            try
            {
                Reserva r = Reserva.ConsultarReservaPorId(id);

                rResponse.Reserva.Data = r.Data.ToString("ddMMyyyy");
                rResponse.Reserva.Horario = r.Horario;
                rResponse.Reserva.Turno = r.Turno;
                rResponse.Reserva.Obs = r.Obs;
                rResponse.Reserva.ReservadoEm = r.ReservadoEm.ToString("ddMMyyyy HHmm");
                rResponse.Reserva.NomeLocal = r.Local.Nome;
                rResponse.Reserva.NomeUsuario = r.Usuario.Nome;
                rResponse.Reserva.EmailUsuario = r.Usuario.Email;
                foreach (CategoriaEquipamento ce in r.CategoriasEquipamentos)
                {
                    rResponse.Reserva.Equipamentos.Add(ce.Nome);
                }
            }
            catch (EntidadesException eex)
            {
                rResponse.Status = (int)eex.Codigo;
                rResponse.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                rResponse.Status = -1;
                rResponse.Detalhes = ex.Message;
            }
            return Ok(rResponse);
        }

        // POST api/<controller>
        [Route("")]
        public IHttpActionResult Post([FromBody]ReservaRegistroTO reserva)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                DateTime data = DateTime.ParseExact(reserva.Data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dataReservado = DateTime.ParseExact(reserva.ReservadoEm, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                Reserva.Reservar(data, reserva.Horario, reserva.Turno, reserva.IdLocal, reserva.Obs, reserva.IdCategoria);
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
            BaseResponse bResponse = new BaseResponse();
            try
            {
                Reserva.Remover(id);
            }
            catch (EntidadesException eex)
            {
                bResponse.Status = (int)eex.Codigo;
                bResponse.Detalhes = eex.Message;
            }
            catch (Exception ex)
            {
                bResponse.Status = -1;
                bResponse.Detalhes = ex.Message;
            }
            return Ok(bResponse);
        }
    }
}