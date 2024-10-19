
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Cundi_incidencias.Dto;
using System.Net.Mail;
using System.Net;
using Cundi_incidencias.Utility;
using System.Reflection.Metadata.Ecma335;

namespace Cundi_incidecnias.Utility
{
    public class CorreoUtility
    {
        private SmtpClient cliente;
        private MailMessage email;
        private string Host = "smtp.gmail.com";
        private int Port = 587;
        private string User = "incidenciascundi@gmail.com";
        private string Password = "iqbwggcgaptgnzpt";
        private bool EnabledSSL = true;
        public CorreoUtility()
        {
            cliente = new SmtpClient(Host, Port)
            {
                EnableSsl = EnabledSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(User, Password)
            };
        }
        public void EnviarCorreo(string destinatario, string asunto, string mensajeCorreo, bool esHtlm = true)
        {
            email = new MailMessage(User, destinatario, asunto, mensajeCorreo);
            email.IsBodyHtml = esHtlm;
            cliente.Send(email);

        }

        public string enviarCorreoContrasena(string destinatario, string clave)
        {

            CodigoRecuperacionUtility codigoRecuperacionUtility = new CodigoRecuperacionUtility();
            UsuarioInDto usuario = new UsuarioInDto();
            String codigo = clave;
            String mensajeCorreo = mensajeCon(codigo);
            EnviarCorreo(destinatario, "Cambiar Contraseña", mensajeCorreo, true);
            return codigo;

        }
        public int enviarCorreoCuenta(string destinatario, int clave)
        {

            CodigoRecuperacionUtility codigoRecuperacionUtility = new CodigoRecuperacionUtility();
            UsuarioInDto usuario = new UsuarioInDto();
            int codigo = clave;
            String mensajeCorreo = mensajeActi(codigo);
            EnviarCorreo(destinatario, "Activar Cuenta", mensajeCorreo, true);
            return codigo;

        }


        public string mensajeCon(string codigo)
        {
            string mensajeCon = "<html>" +
"<head>" +
"<style>" +
"body {" +
"margin: 0;" +
"padding: 0;" +
"font-family: Arial, sans-serif;" +
"background-color: #ffffff;" +
"}" +
".container {" +
"max-width: 30rem;" +
"margin: 0 auto;" +
"padding: 2rem;" +
"background-color: #ffffff;" +
"border-radius: 1rem;" +
"box-shadow: 0 0 2rem rgba(0, 128, 0, 0.2);" +
"color: #000000;" +
"}" +

".header {" +
"text-align: center;" +
"background-color: #008000;" +
"padding: 1rem;" +
"border-top-left-radius: 1rem;" +
"border-top-right-radius: 1rem;" +
"color: #ffffff;" +
"}" +
".logo {" +
"width: 80px;" +
"vertical-align: middle;" +
"margin-right: 0.5rem;" +
"}" +
".header h1 {" +
"display: inline-block;" +
"vertical-align: middle;" +
"margin: 0;" +
"font-size: 1.5rem;" +
"}" +

".content {" +
"padding: 1rem;" +
"background-color: #ffffff;" +
"border-radius: 1rem;" +
"box-shadow: 0 2px 5px rgba(0, 128, 0, 0.1);" +
"border: 1px solid #008000;" +
"}" +
".content h2 {" +
"color: #008000;" +
"margin-bottom: 1.3rem;" +
"}" +
".content p {" +
"color: #000000;" +
"font-size: 1rem;" +
"line-height: 1.6;" +
"}" +

".footer {" +
"text-align: center;" +
"margin-top: 2rem;" +
"padding-top: 1rem;" +
"border-top: 1px solid #008000;" +
"}" +
".footer p {" +
"color: #008000;" +
"font-size: 0.8rem;" +
"}" +
".codigo {" +
"text-align: center;" +
"font-weight: bold;" +
"color: #008000;" +
"font-size: 1.2rem;" +
"}" +
"</style>" +
"</head>" +
"<body>" +

"<div class='container'>" +
"<div class='header'>" +
"<img src='https://i.ibb.co/f8ytCBW/Cundi.png' alt='Cundi-Incidencias' class='logo'/>" +
"<h1>Cundi-Incidencias</h1>" +
"</div>" +

"<div class='content'>" +
"<h2>¡Hola!</h2>" +
"<p>Recibimos una solicitud para restablecer tu contraseña.</p>" +
"<p>A continuación, encontrarás el código que necesitas para continuar con el proceso de cambio de contraseña:</p>" +
"<p class='codigo'>" + codigo + "</p>" +
"<p>Si no solicitaste este cambio, puedes ignorar este correo y tu contraseña actual se mantendrá sin cambios.</p>" +
"<p>Gracias por utilizar Cundi-Incidencias.</p>" +
"</div>" +

"<div class='footer'>" +
"<p>Este correo electrónico fue enviado por Cundi-Incidencias. Para cualquier consulta, escríbenos a cundiincidenciassoporte@gmail.com.</p>" +
"</div>" +
"</div>" +

"</body>" +
"</html>";


            return mensajeCon;
        }



        public string mensajeActi(int token)
        {
            string mensajeActi = $"<html>" +
            "<head>" +
            "<style>" +
            "body {" +
            "margin: 0;" +
            "padding: 0;" +
            "font-family: Arial, sans-serif;" +
            "background-color: #ffffff;" +
            "}" +
            ".container {" +
            "max-width: 30rem;" +
            "margin: 0 auto;" +
            "padding: 2rem;" +
            "background-color: #ffffff;" +
            "border-radius: 1rem;" +
            "box-shadow: 0 0 2rem rgba(0, 128, 0, 0.2);" +
            "color: #000000;" +
            "}" +

            ".header {" +
            "text-align: center;" +
            "background-color: #008000;" +
            "padding: 1rem;" +
            "border-top-left-radius: 1rem;" +
            "border-top-right-radius: 1rem;" +
            "color: #ffffff;" +
            "}" +
            ".logo {" +
            "width: 80px;" +
            "vertical-align: middle;" +
            "margin-right: 0.5rem;" +
            "}" +
            ".header h1 {" +
            "display: inline-block;" +
            "vertical-align: middle;" +
            "margin: 0;" +
            "font-size: 1.5rem;" +
            "}" +

            ".content {" +
            "padding: 1rem;" +
            "background-color: #ffffff;" +
            "border-radius: 1rem;" +
            "box-shadow: 0 2px 5px rgba(0, 128, 0, 0.1);" +
            "border: 1px solid #008000;" +
            "}" +
            ".content h2 {" +
            "color: #008000;" +
            "margin-bottom: 1.3rem;" +
            "}" +
            ".content p {" +
            "color: #000000;" +
            "font-size: 1rem;" +
            "line-height: 1.6;" +
            "}" +

            ".footer {" +
            "text-align: center;" +
            "margin-top: 2rem;" +
            "padding-top: 1rem;" +
            "border-top: 1px solid #008000;" +
            "}" +
            ".footer p {" +
            "color: #008000;" +
            "font-size: 0.8rem;" +
            "}" +
            ".codigo {" +
            "text-align: center;" +
            "font-weight: bold;" +
            "color: #008000;" +
            "font-size: 1.2rem;" +
            "}" +
            "</style>" +
            "</head>" +
            "<body>" +

            "<div class='container'>" +
            "<div class='header'>" +
            "<img src='https://i.ibb.co/f8ytCBW/Cundi.png' alt='Cundi-Incidencias' class='logo'/>" +
            "<h1>Cundi-Incidencias</h1>" +
            "</div>" +

            "<div class='content'>" +
            "<h2>¡Hola!</h2>" +
            "<p>Bienvenido a Cundi-incidencias.</p>" +
            "<p>Para activar tu cuenta, haz clic en el siguiente enlace:</p>" +

            $"<p style='text-align: center;'>" +
            $"<a href='https://localhost:7124/api/Usuario/ActualizarCuenta?token={token}' " +
            "style='display: inline-block; padding: 0.8rem 1.5rem; background-color: #008000; color: #ffffff; text-decoration: none; border-radius: 0.5rem;' onclick='activarCuenta({token})'>Activar Cuenta</a>" +
            "</p>" +

            "<p>Si no solicitaste este cambio, puedes ignorar este correo y tu cuenta se mantendrá inactiva.</p>" +
            "<p>Gracias por utilizar Cundi-Incidencias.</p>" +
            "</div>" +

            "<div class='footer'>" +
            "<p>Este correo electrónico fue enviado por Cundi-Incidencias. Para cualquier consulta, escríbenos a cundiincidenciassoporte@gmail.com.</p>" +
            "</div>" +
            "</div>" +

            "</body>" +
            "</html>";
            return mensajeActi;
        }


    }
}

