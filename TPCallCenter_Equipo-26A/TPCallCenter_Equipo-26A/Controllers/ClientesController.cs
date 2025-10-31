using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TPCallCenter_Equipo_26A.Controllers
{
    public class ClientesController : Controller
    {
        // GET: Clientes
        public ActionResult Index()
        {
            ViewBag.Title = "Gestión de Clientes";
            ViewBag.Message = "Esta es la página de Clientes funcionando correctamente!";
            
            // Retornamos datos de prueba simples
            var clientes = new List<dynamic>
            {
                new { IDCliente = 1, Nombre = "Juan", Apellido = "Pérez", Email = "juan@email.com" },
                new { IDCliente = 2, Nombre = "María", Apellido = "González", Email = "maria@email.com" }
            };
            
            return View(clientes);
        }

        // GET: Clientes/Test
        public ActionResult Test()
        {
            return Content("El controlador Clientes funciona correctamente!");
        }
    }
}