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
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            UsuariosResponse response = new UsuariosResponse();
            response.Usuarios = new List<UsuarioTO>();

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

                    response.Usuarios.Add(uTO);
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
            UsuarioResponse response = new UsuarioResponse();

            try
            {
                Usuario u = Usuario.ConsultarUsuarioPorId(id);
                response.Usuario = new UsuarioTO();
                response.Usuario.Id = u.Id;
                response.Usuario.Nome = u.Nome;
                response.Usuario.Email = u.Email;
                response.Usuario.Tipo = (int)u.Tipo;
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
            UsuarioResponse response = new UsuarioResponse();

            try
            {
                Usuario u = Usuario.ConsultarUsuarioPorEmail(email);
                response.Usuario = new UsuarioTO();
                response.Usuario.Id = u.Id;
                response.Usuario.Nome = u.Nome;
                response.Usuario.Email = u.Email;
                response.Usuario.Tipo = (int)u.Tipo;
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
            UsuarioResponse response = new UsuarioResponse();
            response.Usuario = usuario;

            try
            {
                response.Usuario.Id = Usuario.Cadastrar(usuario.Nome, usuario.Email, (TIPOUSUARIO)usuario.Tipo);
                
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
