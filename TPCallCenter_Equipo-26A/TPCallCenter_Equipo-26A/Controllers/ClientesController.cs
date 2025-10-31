using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dominio;

namespace TPCallCenter_Equipo_26A.Controllers
{
    public class ClientesController : Controller
    {
        // GET: Clientes
        public ActionResult Index()
        {
            ViewBag.Title = "Gestión de Clientes";
            ViewBag.Message = "Esta es la página de Clientes funcionando correctamente!";
            
            // Crear objetos Clientes reales en lugar de objetos anónimos
            var clientes = new List<Clientes>
            {
                new Clientes
                {
                    IDCliente = 1,
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Documento = "12345678",
                    Email = "juan@email.com",
                    Telefono = "1234567890",
                    Direccion = "Av. Siempre Viva 123",
                    Activo = true,
                    fechaAlta = DateTime.Now.AddDays(-30)
                },
                new Clientes
                {
                    IDCliente = 2,
                    Nombre = "María",
                    Apellido = "González",
                    Documento = "87654321",
                    Email = "maria@email.com",
                    Telefono = "0987654321",
                    Direccion = "Calle Falsa 456",
                    Activo = true,
                    fechaAlta = DateTime.Now.AddDays(-15)
                }
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