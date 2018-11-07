using System;
using System.Collections.Generic;
using System.Linq;
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
                if(dbModel.Usuario.Any( x => x.correo == userModel.correo))
                {
                    ViewBag.DuplicateMessage ="El usuario ya existe";
                    return View("AddOrEdit", userModel);
                }
                dbModel.Usuario.Add(userModel);
                dbModel.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccesMessage = "Registro completado!";
            return (View("AddOrEdit", new Usuario()));
        }
    }
}