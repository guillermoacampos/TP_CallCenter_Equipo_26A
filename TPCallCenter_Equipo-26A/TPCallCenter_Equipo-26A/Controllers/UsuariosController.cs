using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using negocio;
using dominio;


namespace TPCallCenter_Equipo_26A.Controllers
{
    public class UsuariosController : ApiController
    {
        private UsuariosNegocio usuariosNegocio = new UsuariosNegocio();

        // GET api/usuarios
        public IHttpActionResult Get()
        {
            try
            {
                List<Usuarios> usuarios = usuariosNegocio.listar();
                
                if (usuarios == null || usuarios.Count == 0)
                {
                    return Ok(new { message = "No se encontraron usuarios", data = new List<Usuarios>() });
                }

                return Ok(new { message = "Usuarios obtenidos correctamente", data = usuarios });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener los usuarios: " + ex.Message));
            }
        }

        // GET api/usuarios/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                Usuarios usuario = usuariosNegocio.obtenerPorId(id);
                
                if (usuario == null)
                {
                    return NotFound();
                }

                return Ok(new { message = "Usuario obtenido correctamente", data = usuario });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener el usuario: " + ex.Message));
            }
        }
    }
}