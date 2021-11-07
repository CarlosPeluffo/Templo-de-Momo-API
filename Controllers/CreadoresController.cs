using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Templo_de_Momo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Templo_de_Momo.Controllers
{
    public class CreadoresController : Controller
    {
        private readonly IRepositorioCreador repositorio;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        public CreadoresController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioCreador repositorio)
        {
            this.repositorio = repositorio;
            this.configuration = configuration;
            this.environment = environment;
        }

        // GET: Creadores
        public ActionResult Index()
        {
            try
            {
                if(TempData.ContainsKey("Mensaje")){
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                if(TempData.ContainsKey("Error")){
                    ViewBag.Mensaje = TempData["Error"];
                }
                if(TempData.ContainsKey("StackTrate")){
                    ViewBag.StackTrate = TempData["StackTrate"];
                }
                var lista = repositorio.ObtenerTodos();
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        // GET: Creadores/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Creadores/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Creadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Creador creador)
        {
            try
            {
                if(ModelState.IsValid){
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: creador.Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256/8));
                    creador.Password = hashed;
                    int res = repositorio.Alta(creador);
                    if(creador.AvatarFile != null && creador.Id > 0){
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "UsersFiles");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "photo_" + creador.Id + Path.GetExtension(creador.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        creador.Avatar = Path.Combine("/UsersFiles", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)){
                        creador.AvatarFile.CopyTo(stream);
                        }
                        repositorio.Modificacion(creador);
                    }
                    TempData["Id"] = creador.Id;
                    return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo cargar";
                    return View(creador);
                }
                
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Creadores/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                ViewData["Title"] = "Editar Creador";
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Creadores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Creador creador)
        {
            try
            {
                var OldCreador = repositorio.ObtenerPorId(id);
                if(creador.Password == null){
                    creador.Password = OldCreador.Password;
                }
                if(creador.Password != OldCreador.Password){
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: creador.Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256/8));
                    creador.Password = hashed;
                }
                if(creador.AvatarFile != null){
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "UsersFiles");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "photo_" + creador.Id + Path.GetExtension(creador.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    creador.Avatar = Path.Combine("/UsersFiles", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)){
                    creador.AvatarFile.CopyTo(stream);
                    }
                }else{
                    creador.Avatar = OldCreador.Avatar;
                }
                repositorio.Modificacion(creador);
                TempData["Mensaje"] = "El Creador se modificó Correctamente";
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Creadores/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Creadores/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Creador creador)
        {
            try
            {
                repositorio.Baja(creador);
                TempData["Mensaje"] = "El Creador se eliminó con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}