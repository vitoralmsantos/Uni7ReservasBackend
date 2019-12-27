using System;
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
    [RoutePrefix("api/categoria")]
    public class CategoriaController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            EntidadesResponse<CategoriaTO> response = new EntidadesResponse<CategoriaTO>();

            try
            {
                List<CategoriaEquipamento> categorias = CategoriaEquipamento.ConsultarCategorias();

                foreach (CategoriaEquipamento ce in categorias)
                {
                    CategoriaTO cTO = new CategoriaTO();
                    cTO.Id = ce.Id;
                    cTO.Nome = ce.Nome;
                    cTO.ComentarioReserva = ce.ComentarioReserva;

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
            EntidadeResponse<CategoriaTO> response = new EntidadeResponse<CategoriaTO>();

            try
            {
                CategoriaEquipamento ce = CategoriaEquipamento.ConsultarCategoriaPorId(id);
                response.Elemento = new CategoriaTO();
                response.Elemento.Id = ce.Id;
                response.Elemento.Nome = ce.Nome;
                response.Elemento.ComentarioReserva = ce.ComentarioReserva;
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
            EntidadesResponse<CategoriaTO> response = new EntidadesResponse<CategoriaTO>();
            try
            {
                DateTime d = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                List<CategoriaEquipamento> categorias = CategoriaEquipamento.ConsultarCategoriasDisponiveis(d, horario, turno);

                foreach (CategoriaEquipamento ce in categorias)
                {
                    CategoriaTO cTO = new CategoriaTO();
                    cTO.Id = ce.Id;
                    cTO.Nome = ce.Nome;
                    cTO.ComentarioReserva = ce.ComentarioReserva;

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

        [Route("")]
        public IHttpActionResult Post([FromBody]CategoriaTO categoria)
        {
            EntidadeResponse<CategoriaTO> response = new EntidadeResponse<CategoriaTO>();
            response.Elemento = categoria;

            try
            {
                response.Elemento.Id = CategoriaEquipamento.Cadastrar(categoria.Nome, categoria.ComentarioReserva);
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
        public IHttpActionResult Put(int id, [FromBody]CategoriaTO categoria)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                CategoriaEquipamento.Atualizar(id, categoria.Nome, categoria.ComentarioReserva);
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
                CategoriaEquipamento.Remover(id);
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
