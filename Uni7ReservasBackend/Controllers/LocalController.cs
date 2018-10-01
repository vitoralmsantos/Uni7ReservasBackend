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
            LocaisResponse response = new LocaisResponse();
            response.Locais = new List<LocalTO>();

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

                    response.Locais.Add(lTO);
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
            LocalResponse response = new LocalResponse();

            try
            {
                Local l = Local.ConsultarPorId(id);
                response.Local = new LocalTO();
                response.Local.Id = l.Id;
                response.Local.Nome = l.Nome;
                response.Local.Disponivel = l.Disponivel;
                response.Local.Reservavel = l.Reservavel;
                response.Local.Tipo = (int)l.Tipo;
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
            LocaisResponse response = new LocaisResponse();

            try
            {
                DateTime d = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                List<Local> locais = Reserva.ConsultarLocaisDisponiveis(d, horario, turno, false);
                foreach (Local l in locais)
                {
                    LocalTO lTO = new LocalTO();
                    lTO.Id = l.Id;
                    lTO.Nome = l.Nome;
                    lTO.Disponivel = l.Disponivel;
                    lTO.Reservavel = l.Reservavel;
                    lTO.Tipo = (int)l.Tipo;

                    response.Locais.Add(lTO);
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
            LocalResponse response = new LocalResponse();

            try
            {
                Local.Cadastrar(local.Nome, local.Reservavel, local.Disponivel, (TIPOLOCAL)local.Tipo);
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
            LocalResponse response = new LocalResponse();

            try
            {
                Local.Atualizar(id, local.Nome, local.Reservavel, local.Disponivel, (TIPOLOCAL)local.Tipo);
                response.Local = local;
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
