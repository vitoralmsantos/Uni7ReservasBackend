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
    [RoutePrefix("api/local")]
    public class LocalController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            EntidadesResponse<LocalTO> response = new EntidadesResponse<LocalTO>();

            try
            {
                List<Local> locais = Local.Consultar();

                foreach (Local l in locais)
                {
                    LocalTO lTO = new LocalTO();
                    lTO.Id = l.Id;
                    lTO.Nome = l.Nome;
                    lTO.Disponivel = l.Disponivel;
                    lTO.Reservavel = l.Reservavel;
                    lTO.Tipo = (int)l.Tipo;
                    lTO.ComentarioReserva = l.ComentarioReserva;

                    response.Elementos.Add(lTO);
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

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            EntidadeResponse<LocalTO> response = new EntidadeResponse<LocalTO>();

            try
            {
                Local l = Local.ConsultarPorId(id);
                response.Elemento = new LocalTO();
                response.Elemento.Id = l.Id;
                response.Elemento.Nome = l.Nome;
                response.Elemento.Disponivel = l.Disponivel;
                response.Elemento.Reservavel = l.Reservavel;
                response.Elemento.Tipo = (int)l.Tipo;
                response.Elemento.ComentarioReserva = l.ComentarioReserva;
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

        [Route("disponibilidade")]
        public IHttpActionResult Get([FromUri]string data, [FromUri]string horario, [FromUri]string turno)
        {
            EntidadesResponse<LocalTO> response = new EntidadesResponse<LocalTO>();

            try
            {
                DateTime d = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                List<Local> locais = Local.ConsultarLocaisDisponiveis(d, horario, turno, false);
                foreach (Local l in locais)
                {
                    LocalTO lTO = new LocalTO();
                    lTO.Id = l.Id;
                    lTO.Nome = l.Nome;
                    lTO.Disponivel = l.Disponivel;
                    lTO.Reservavel = l.Reservavel;
                    lTO.Tipo = (int)l.Tipo;
                    lTO.ComentarioReserva = l.ComentarioReserva;

                    response.Elementos.Add(lTO);
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

        [Route("")]
        public IHttpActionResult Post([FromBody]LocalTO local)
        {
            EntidadeResponse<LocalTO> response = new EntidadeResponse<LocalTO>();

            try
            {
                response.Elemento.Id = Local.Cadastrar(local.Nome, local.Reservavel, local.Disponivel, (TIPOLOCAL)local.Tipo,
                    local.ComentarioReserva);
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

        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody]LocalTO local)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                Local.Atualizar(id, local.Nome, local.Reservavel, local.Disponivel, (TIPOLOCAL)local.Tipo, local.ComentarioReserva);
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

        // DELETE: api/Usuario/5
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                Local.Remover(id);
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
