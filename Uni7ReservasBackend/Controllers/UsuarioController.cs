using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Uni7ReservasBackend.Controllers.Response;
using Uni7ReservasBackend.Controllers.TransferObjects;
using Uni7ReservasBackend.Models;
using Uni7ReservasBackend.Models.Entidades;

namespace Uni7ReservasBackend.Controllers
{
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        // GET api/<controller>
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

        // GET: api/Usuario/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Usuario
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Usuario/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Usuario/5
        public void Delete(int id)
        {
        }
    }
}
