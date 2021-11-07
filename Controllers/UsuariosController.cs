using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Templo_de_Momo.Models;

namespace Templo_de_Momo.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IRepositorioUsuario repositorio;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioUsuario repositorio)
        {
            this.repositorio = repositorio;
            this.configuration = configuration;
            this.environment = environment;
        }
        // GET: Usuarios
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

        // GET: Usuarios/Details/5
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

        // GET: Usuarios/Create
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

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            try
            {
                if(ModelState.IsValid){
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256/8));
                    usuario.Password = hashed;
                    int res = repositorio.Alta(usuario);
                    if(usuario.AvatarFile != null && usuario.Id > 0){
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "UsersFiles");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "photo_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        usuario.Avatar = Path.Combine("/UsersFiles", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)){
                        usuario.AvatarFile.CopyTo(stream);
                        }
                        repositorio.Modificacion(usuario);
                    }
                    TempData["Id"] = usuario.Id;
                    return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo cargar";
                    return View(usuario);
                }
                
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                ViewData["Title"] = "Editar Usuario";
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

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario usuario)
        {
             try
            {
                var OldUsuario = repositorio.ObtenerPorId(id);
                if(usuario.Password == null){
                    usuario.Password = OldUsuario.Password;
                }
                if(usuario.Password != OldUsuario.Password){
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256/8));
                    usuario.Password = hashed;
                }
                if(usuario.AvatarFile != null){
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "UsersFiles");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "photo_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/UsersFiles", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)){
                    usuario.AvatarFile.CopyTo(stream);
                    }
                }else{
                    usuario.Avatar = OldUsuario.Avatar;
                }
                repositorio.Modificacion(usuario);
                TempData["Mensaje"] = "El Usuario se modificó Correctamente";
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Usuarios/Delete/5
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

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                repositorio.Baja(usuario);
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