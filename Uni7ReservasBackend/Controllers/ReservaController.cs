using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Uni7ReservasBackend.Controllers.Response;
using Uni7ReservasBackend.Controllers.TransferObjects;
using Uni7ReservasBackend.Models;
using Uni7ReservasBackend.Models.Entidades;

namespace Uni7ReservasBackend.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/reserva")]
    public class ReservaController : ApiController
    {
        // GET api/<controller>
        [Route("")]
        public IHttpActionResult Get()
        {
            ReservasResponse rResponse = new ReservasResponse();
            try
            {
                List<Reserva> reservas = Reserva.ConsultarReservas();

                foreach (Reserva r in reservas)
                {
                    
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
            
            return Ok();
        }

        [Route("{id:int}/produto")]
        public IHttpActionResult GetProdutosPorCategoria(int id)
        {
            
            return Ok();
        }

        // POST api/<controller>
        public IHttpActionResult Post([FromBody]string categoria)
        {
            BaseResponse response = new BaseResponse();
            try
            {
               
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
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]string categoria)
        {
            
            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            BaseResponse bResponse = new BaseResponse();
            try
            {
                
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
