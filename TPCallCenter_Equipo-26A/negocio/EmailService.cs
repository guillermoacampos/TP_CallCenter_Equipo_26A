using System;
using System.Net;
using System.Net.Mail;

namespace negocio
{
    public class EmailService
    {
        private MailMessage email;
        private SmtpClient server;

        public EmailService()
        {
            server = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("callCenter_tpi@hotmail.com", "contraseña_de_aplicación"),
                //TargetName = "STARTTLS/smtp-mail.outlook.com",
            };
        }

        public void armarCorreo(string emailDestino, string asunto, string cuerpo)
        {
            email = new MailMessage
            {
                From = new MailAddress("noresponder@callCenter_tpi.com"),
                Subject = asunto,
                IsBodyHtml = true,
                Body = cuerpo
            };
            email.To.Add(emailDestino);
        }

        public void enviarEmail()
        {
            try
            {
                server.Send(email);
                Console.WriteLine("Correo enviado exitosamente a: " + email.To[0].Address);
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine("Error SMTP: " + smtpEx.Message);
                Console.WriteLine("Estado del servidor SMTP: " + smtpEx.StatusCode);
                Console.WriteLine("Detalles adicionales: " + smtpEx.InnerException?.Message);
                throw new Exception("Error al enviar el correo: " + smtpEx.Message, smtpEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error general: " + ex.Message);
                Console.WriteLine("Detalles del error: " + ex.StackTrace);
                throw new Exception("Error al enviar el correo: " + ex.Message, ex);
            }
        }
    }
}