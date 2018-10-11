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
    [RoutePrefix("api/recurso")]
    public class RecursoController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            RecursosResponse response = new RecursosResponse();
            response.Recursos = new List<RecursoTO>();

            try
            {
                List<Recurso> recursos = Recurso.Consultar();

                foreach (Recurso r in recursos)
                {
                    RecursoTO rTO = new RecursoTO();
                    rTO.Id = r.Id;
                    rTO.Nome = r.Nome;
                    rTO.Detalhes = r.Detalhes;
                    rTO.Vencimento = r.Vencimento.HasValue ? r.Vencimento.Value.ToString("dd/MM/yyyy") : "";
                    rTO.Tipo = (int)r.Tipo;

                    response.Recursos.Add(rTO);
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
            RecursoResponse response = new RecursoResponse();

            try
            {
                Recurso r = Recurso.ConsultarPorId(id);
                response.Recurso = new RecursoTO();
                response.Recurso.Id = r.Id;
                response.Recurso.Nome = r.Nome;
                response.Recurso.Detalhes = r.Detalhes;
                response.Recurso.Vencimento = r.Vencimento.HasValue ? r.Vencimento.Value.ToString("dd/MM/yyyy") : "";
                response.Recurso.Tipo = (int)r.Tipo;
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
        public IHttpActionResult Post([FromBody]RecursoTO recurso)
        {
            RecursoResponse response = new RecursoResponse();
            response.Recurso = recurso;

            try
            {
                DateTime? vencimento = null;
                if (!recurso.Vencimento.Equals(""))
                {
                    vencimento = DateTime.ParseExact(recurso.Vencimento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                response.Recurso.Id = Recurso.Cadastrar(recurso.Nome, recurso.Detalhes, vencimento, (TIPORECURSO)recurso.Tipo);   
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
        public IHttpActionResult Put(int id, [FromBody]RecursoTO recurso)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                DateTime? vencimento = null;
                if (!recurso.Vencimento.Equals(""))
                {
                    vencimento = DateTime.ParseExact(recurso.Vencimento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                Recurso.Atualizar(id, recurso.Nome, recurso.Detalhes, vencimento, (TIPORECURSO)recurso.Tipo);
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
                Recurso.Remover(id);
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

        [Route("cadastrarlocal")]
        [HttpPost]
        public IHttpActionResult CadastrarPorLocal([FromBody]RecursoLocalTO recursoLocal)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                Recurso.CadastrarPorLocal(recursoLocal.IdRecurso, recursoLocal.IdLocal, recursoLocal.Qtde);
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

        [Route("removerlocal")]
        [HttpPost]
        public IHttpActionResult RemoverPorLocal([FromBody]RecursoLocalTO recursoLocal)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                Recurso.RemoverPorLocal(recursoLocal.IdRecurso, recursoLocal.IdLocal);
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
