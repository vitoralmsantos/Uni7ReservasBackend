﻿using System;
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
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            EntidadesResponse<UsuarioTO> response = new EntidadesResponse<UsuarioTO>();

            try
            {
                List<Usuario> usuarios = Usuario.ConsultarUsuarios();

                foreach (Usuario u in usuarios)
                {
                    UsuarioTO uTO = new UsuarioTO();
                    uTO.Id = u.Id;
                    uTO.Nome = u.Nome;
                    uTO.Email = u.Email;
                    uTO.Tipo = (int)u.Tipo;

                    response.Elementos.Add(uTO);
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

        [Route("consulta/{id:int}")]
        public IHttpActionResult Get(int id)
        {
            EntidadeResponse<UsuarioTO> response = new EntidadeResponse<UsuarioTO>();

            try
            {
                Usuario u = Usuario.ConsultarUsuarioPorId(id);
                response.Elemento = new UsuarioTO();
                response.Elemento.Id = u.Id;
                response.Elemento.Nome = u.Nome;
                response.Elemento.Email = u.Email;
                response.Elemento.Tipo = (int)u.Tipo;
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

        [Route("consulta")]
        public IHttpActionResult Get([FromUri]string email)
        {
            EntidadeResponse<UsuarioTO> response = new EntidadeResponse<UsuarioTO>();

            try
            {
                Usuario u = Usuario.ConsultarUsuarioPorEmail(email);
                response.Elemento = new UsuarioTO();
                response.Elemento.Id = u.Id;
                response.Elemento.Nome = u.Nome;
                response.Elemento.Email = u.Email;
                response.Elemento.Tipo = (int)u.Tipo;
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

        // POST: api/Usuario
        [Route("")]
        public IHttpActionResult Post([FromBody]UsuarioTO usuario)
        {
            EntidadeResponse<UsuarioTO> response = new EntidadeResponse<UsuarioTO>();
            response.Elemento = usuario;

            try
            {
                response.Elemento.Id = Usuario.Cadastrar(usuario.Nome, usuario.Email, (TIPOUSUARIO)usuario.Tipo);
                
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

        // POST: api/Usuario
        [Route("login")]
        public IHttpActionResult Autenticar([FromBody]string email, [FromBody]string senha)
        {
            TokenResponse response = new TokenResponse();
            
            try
            {
                response.Token = Usuario.Autenticar(email, senha);
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

        // PUT: api/Usuario/5
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody]UsuarioTO usuario)
        {
            BaseResponse response = new BaseResponse();
            
            try
            {
                Usuario.Atualizar(id, usuario.Nome, usuario.Email, (TIPOUSUARIO)usuario.Tipo);
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
                Usuario.Remover(id);
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
