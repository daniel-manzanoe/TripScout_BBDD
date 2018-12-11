using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Registro;

namespace Registro.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            Usuario usuarioModel = new Usuario();
            return View(usuarioModel);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Usuario userModel)
        {
            using (DbModels dbModel = new DbModels())
            {
                if (dbModel.Usuario.Any(x => x.correo == userModel.correo) || !passwordSecurity(userModel.contraseña))
                {
                    Debug.WriteLine("Contraseña invalida");

                    return View("AddOrEdit", new Usuario());

                }
                Debug.WriteLine("Contraseña valida");
                dbModel.Usuario.Add(userModel);
                SendEmail(userModel.correo);
                dbModel.SaveChanges();
            }
            return (View("AddOrEdit", new Usuario()));
        }

        [HttpGet]
        public ActionResult Login(int id = 0)
        {
            Usuario usuarioModel = new Usuario();
            return View(usuarioModel);
        }

        [HttpPost]
        public ActionResult Login(Usuario userModel)
        {
            using (DbModels dbModel = new DbModels())
            {
                if (dbModel.Usuario.Any(x => x.correo == userModel.correo
                 && x.contraseña == userModel.contraseña && x.tipo == true))
                {
                    //usuario guia
                    Debug.WriteLine("Usuario GUIA");
                }
                if (dbModel.Usuario.Any(x => x.correo == userModel.correo
                 && x.contraseña == userModel.contraseña && x.tipo == false))
                {
                    //usuario turista
                    Debug.WriteLine("Usuario TURISTA");
                    return (View("Login", new Usuario()));
                }
            }
            Debug.WriteLine("No existe el usuario");
            return (View("Login", new Usuario()));
        }
        public bool passwordSecurity(String password)
        {
            if (Regex.IsMatch(password, @"^.*(?=.{7,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).*$"))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public void SendEmail(String email)
        {
            MailMessage mail = new MailMessage("tripscout.ISA@gmail.com", email, "Confirmar registro", "mailBody");
            mail.From = new MailAddress("tripscout.ISA@gmail.com", "TripScout");
            mail.IsBodyHtml = true; // necessary if you're using html email

            NetworkCredential credential = new NetworkCredential("tripscout.ISA@gmail.com", "tripscout1234");
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;
            smtp.Send(mail);
        }

        private Boolean email_bien_escrito(String email)
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
