using System;
using dominio;

namespace negocio
{
    public class IncidenciaNotificador
    {
        private readonly EmailService emailService;

        public IncidenciaNotificador()
        {
            emailService = new EmailService();
        }

        public void NotificarCambioEstado(Incidencias incidencia, string accion)
        {
            string asunto = "";
            string cuerpo = "";

            switch (accion)
            {
                case "Crear":
                    asunto = $"Alta de Incidencia - N° {incidencia.NumReclamo}";
                    cuerpo = $"Hola {incidencia.Cliente.Nombre},\n\nSe ha registrado una nueva incidencia con el número de reclamo {incidencia.NumReclamo}.\nDescripción: {incidencia.Descripcion}.\n\nGracias por contactarnos.";
                    break;

                case "Resolver":
                    asunto = $"Resolución de Incidencia - N° {incidencia.NumReclamo}";
                    cuerpo = $"Hola {incidencia.Cliente.Nombre},\n\nSu incidencia con el número de reclamo {incidencia.NumReclamo} ha sido resuelta.\nComentario: {incidencia.ComentarioResolucion}.\n\nGracias por su paciencia.";
                    break;

                case "Cerrar":
                    asunto = $"Cierre de Incidencia - N° {incidencia.NumReclamo}";
                    cuerpo = $"Hola {incidencia.Cliente.Nombre},\n\nSu incidencia con el número de reclamo {incidencia.NumReclamo} ha sido cerrada.\nComentario: {incidencia.ComentarioCierre}.\n\nGracias por confiar en nosotros.";
                    break;

                default:
                    throw new ArgumentException("Acción no válida para notificar el cambio de estado.");
            }

            emailService.armarCorreo(incidencia.Cliente.Email, asunto, cuerpo);
            emailService.enviarEmail();
        }
    }
}