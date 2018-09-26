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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/equipamento")]
    public class EquipamentoController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            EquipamentosResponse response = new EquipamentosResponse();
            response.Equipamentos = new List<EquipamentoTO>();

            try
            {
                List<Equipamento> equipamentos = Equipamento.Consultar();

                foreach (Equipamento e in equipamentos)
                {
                    EquipamentoTO eTO = new EquipamentoTO();
                    eTO.Id = e.Id;
                    eTO.Modelo = e.Modelo;
                    eTO.Disponivel = e.Disponivel;
                    eTO.IdCategoria = e.CategoriaEquipamento.Id;

                    response.Equipamentos.Add(eTO);
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
            EquipamentoResponse response = new EquipamentoResponse();

            try
            {
                Equipamento e = Equipamento.ConsultarPorId(id);
                response.Equipamento = new EquipamentoTO();
                response.Equipamento.Id = e.Id;
                response.Equipamento.Modelo = e.Modelo;
                response.Equipamento.Disponivel = e.Disponivel;
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
            EquipamentoResponse response = new EquipamentoResponse();
            response.Equipamento = equipamento;

            try
            {
                response.Equipamento.Id = Equipamento.Cadastrar(equipamento.Modelo, equipamento.Serie, equipamento.Disponivel, equipamento.IdCategoria);
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
