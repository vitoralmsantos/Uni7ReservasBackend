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
    [RoutePrefix("api/chamado")]
    public class ChamadoController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            EntidadesResponse<ChamadoTO> response = new EntidadesResponse<ChamadoTO>();

            try
            {
                List<Chamado> chamados = Chamado.Consultar();

                foreach (Chamado c in chamados)
                {
                    ChamadoTO cTO = new ChamadoTO();
                    cTO.Id = c.Id;
                    cTO.Descricao = c.Descricao;
                    cTO.Observacoes = c.Observacoes;
                    cTO.Status = (int)c.Status;
                    cTO.DataPrevista = c.DataPrevista.HasValue ? c.DataPrevista.Value.ToString("ddMMyyyy") : "";
                    cTO.DataLimite = c.DataLimite.ToString("ddMMyyyy");
                    cTO.Telefone = c.Telefone;

                    response.Elementos.Add(cTO);
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
            EntidadeResponse<ChamadoTO> response = new EntidadeResponse<ChamadoTO>();

            try
            {
                Chamado c = Chamado.ConsultarPorId(id);
                response.Elemento = new ChamadoTO();
                response.Elemento.Id = c.Id;
                response.Elemento.Descricao = c.Descricao;
                response.Elemento.Observacoes = c.Observacoes;
                response.Elemento.Status = (int)c.Status;
                response.Elemento.DataPrevista = c.DataPrevista.HasValue ? c.DataPrevista.Value.ToString("ddMMyyyy") : "";
                response.Elemento.DataLimite = c.DataLimite.ToString("ddMMyyyy");
                response.Elemento.Telefone = c.Telefone;
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
        public IHttpActionResult Post([FromBody]ChamadoTO chamado)
        {
            EntidadeResponse<ChamadoTO> response = new EntidadeResponse<ChamadoTO>();
            response.Elemento = chamado;

            try
            {
                DateTime dataLimite = DateTime.ParseExact(chamado.DataLimite, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                response.Elemento.Id = Chamado.Cadastrar(chamado.Descricao, dataLimite, chamado.Observacoes, chamado.Telefone);
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
        public IHttpActionResult Put(int id, [FromBody]ChamadoTO chamado)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                DateTime dataPrevista = DateTime.ParseExact(chamado.DataPrevista, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Chamado.Atualizar(id, chamado.Observacoes, (STATUSCHAMADO)chamado.Status, dataPrevista);
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
                Chamado.Remover(id);
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
