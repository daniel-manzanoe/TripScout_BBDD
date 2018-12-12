using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using GoogleReCaptchaInMVC5.Models;
using Newtonsoft.Json;
using Registro;
using Registro.Classes;

namespace Registro.Controllers
{
    public class UserController : Controller
    {
        public static CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = System.Web.Configuration.WebConfigurationManager.AppSettings["recaptchaPrivateKey"];
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            Usuario usuarioModel = new Usuario();
            return View(usuarioModel);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Usuario userModel)
        {
            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            Mail mail = new Mail();
            using (DbModels dbModel = new DbModels())
            {
                if (dbModel.Usuario.Any(x => x.correo == userModel.correo))
                {
                    Debug.WriteLine("Ya existe un usuario con ese correo");
                    ViewBag.Error = "Ya existe un usuario con ese correo";
                    return View("AddOrEdit", new Usuario());
                }
                else
                {
                    if (!passwordSecurity(userModel.contraseña))
                    {
                        Debug.WriteLine("La contraseña no es segura");
                        ViewBag.Error = "La contraseña debe tener una longitud de al menos 7 dígitos, 6 caracteres, un caracter en mayúscula" +
                            ", un número y un carácter alfanumérico";

                    }
                    else
                    {
                        if (!mail.email_bien_escrito(userModel.correo))
                        {
                            Debug.WriteLine("correo invalido");
                            ViewBag.Error = "Correo invalido";
                            return View("AddOrEdit", new Usuario());
                        }
                        else
                        {
                            if (response.Success)
                            {
                                Debug.WriteLine("Usuario registrado correctamente");
                                ViewBag.Error = "Usuario registrado correctamente";
                                dbModel.Usuario.Add(userModel);
                                mail.SendEmail(userModel.correo);
                                dbModel.SaveChanges();
                            }
                            else
                                ViewBag.Error = "Captcha incorrecto";
                        }
                    }
                }
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
    }

}
