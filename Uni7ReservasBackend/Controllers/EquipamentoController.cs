using System;
using System.Collections.Generic;
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
    [RoutePrefix("api/equipamento")]
    public class EquipamentoController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            EntidadesResponse<EquipamentoTO> response = new EntidadesResponse<EquipamentoTO>();

            try
            {
                List<Equipamento> equipamentos = Equipamento.Consultar();

                foreach (Equipamento e in equipamentos)
                {
                    EquipamentoTO eTO = new EquipamentoTO();
                    eTO.Id = e.Id;
                    eTO.Modelo = e.Modelo;
                    eTO.Serie = e.Serie;
                    eTO.Disponivel = e.Disponivel;
                    eTO.IdCategoria = e.CategoriaEquipamento.Id;

                    response.Elementos.Add(eTO);
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
            EntidadeResponse<EquipamentoTO> response = new EntidadeResponse<EquipamentoTO>();

            try
            {
                Equipamento e = Equipamento.ConsultarPorId(id);
                response.Elemento = new EquipamentoTO();
                response.Elemento.Id = e.Id;
                response.Elemento.Modelo = e.Modelo;
                response.Elemento.Serie = e.Serie;
                response.Elemento.Disponivel = e.Disponivel;
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
        public IHttpActionResult Post([FromBody]EquipamentoTO equipamento)
        {
            EntidadeResponse<EquipamentoTO> response = new EntidadeResponse<EquipamentoTO>();
            response.Elemento = equipamento;

            try
            {
                response.Elemento.Id = Equipamento.Cadastrar(equipamento.Modelo, equipamento.Serie, equipamento.Disponivel, equipamento.IdCategoria);
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
        public IHttpActionResult Put(int id, [FromBody]EquipamentoTO equipamento)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                Equipamento.Atualizar(id, equipamento.Modelo, equipamento.Serie, equipamento.Disponivel, equipamento.IdCategoria);
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
                Equipamento.Remover(id, true);
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
