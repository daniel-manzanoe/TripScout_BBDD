using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace Registro.Classes
{
    public class Mail
    {
        public Mail()
        {
        }

        public void SendEmail(String email)
        {
            //Carga de la plantilla del correo
            FileInfo plantilla = new FileInfo("C:/correoConfirmacion.html");
            String contenido = plantilla.OpenText().ReadToEnd();

            //Configuración del correo
            MailMessage mail = new MailMessage("tripscout.ISA@gmail.com", email, "Confirmar registro", contenido);
            mail.From = new MailAddress("tripscout.ISA@gmail.com", "TripScout");
            mail.Subject = "Confirma tu registro en TripScout";
            mail.IsBodyHtml = true;             // necessary if you're using html email

            //Configurar cliente smtp y enviar
            NetworkCredential credential = new NetworkCredential("tripscout.ISA@gmail.com", "tripscout1234");
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;
            smtp.Send(mail);
        }

        public Boolean email_bien_escrito(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}