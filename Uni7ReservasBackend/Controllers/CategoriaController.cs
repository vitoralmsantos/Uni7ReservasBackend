﻿using System;
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
    [RoutePrefix("api/categoria")]
    public class CategoriaController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            CategoriasResponse response = new CategoriasResponse();
            response.Categorias = new List<CategoriaTO>();

            try
            {
                List<CategoriaEquipamento> categorias = CategoriaEquipamento.ConsultarCategorias();

                foreach (CategoriaEquipamento ce in categorias)
                {
                    CategoriaTO cTO = new CategoriaTO();
                    cTO.Id = ce.Id;
                    cTO.Nome = ce.Nome;

                    response.Categorias.Add(cTO);
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
            CategoriaResponse response = new CategoriaResponse();

            try
            {
                CategoriaEquipamento ce = CategoriaEquipamento.ConsultarCategoriaPorId(id);
                response.Categoria = new CategoriaTO();
                response.Categoria.Id = ce.Id;
                response.Categoria.Nome = ce.Nome;
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
            CategoriasResponse response = new CategoriasResponse();
            response.Categorias = new List<CategoriaTO>();

            try
            {
                DateTime d = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                List<CategoriaEquipamento> categorias = CategoriaEquipamento.ConsultarCategoriasDisponiveis(d, horario, turno);

                foreach (CategoriaEquipamento ce in categorias)
                {
                    CategoriaTO cTO = new CategoriaTO();
                    cTO.Id = ce.Id;
                    cTO.Nome = ce.Nome;

                    response.Categorias.Add(cTO);
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
            CategoriaResponse response = new CategoriaResponse();
            response.Categoria = categoria;

            try
            {
                response.Categoria.Id = CategoriaEquipamento.Cadastrar(categoria.Nome);
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
                CategoriaEquipamento.Atualizar(id, categoria.Nome);
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
