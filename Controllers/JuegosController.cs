using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Templo_de_Momo.Models;

namespace Templo_de_Momo.Controllers
{
    public class JuegosController : Controller
    {
        private readonly IRepositorioJuego repositorio;
        private readonly IConfiguration configuration;
        private readonly IRepositorioCreador repCreador;
        private readonly IWebHostEnvironment environment;
        public JuegosController(IConfiguration configuration, IWebHostEnvironment environment,IRepositorioCreador repCreador , IRepositorioJuego repositorio)
        {
            this.repositorio = repositorio;
            this.configuration = configuration;
            this.repCreador = repCreador;
            this.environment = environment;
            
        }

        // GET: Juegos
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

        // GET: Juegos/Details/5
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

        // GET: Juegos/Create
        public ActionResult Create()
        {
            try
            {   
                ViewBag.Creadores = repCreador.ObtenerTodos();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Juegos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Juego juego)
        {
            try
            {
                if(ModelState.IsValid){
                    int res = repositorio.Alta(juego);
                    if(juego.PortadaFile != null && juego.Id > 0){
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "UsersFiles");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "portada_" + juego.Id + Path.GetExtension(juego.PortadaFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        juego.Portada = Path.Combine("/UsersFiles", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)){
                        juego.PortadaFile.CopyTo(stream);
                        }
                        repositorio.Modificacion(juego);
                    }
                    TempData["Id"] = juego.Id;
                    return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo cargar";
                    ViewBag.Creadores = repCreador.ObtenerTodos();
                    return View(juego);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Juegos/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                ViewBag.Creadores = repCreador.ObtenerTodos();
                return View(entidad);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Juegos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Juego juego)
        {
            try
            {
                var OldGame = repositorio.ObtenerPorId(id);
                if(ModelState.IsValid){
                    if(juego.PortadaFile != null){
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "UsersFiles");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "portada_" + juego.Id + Path.GetExtension(juego.PortadaFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    juego.Portada = Path.Combine("/UsersFiles", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)){
                    juego.PortadaFile.CopyTo(stream);
                    }
                    }else{
                        juego.Portada = OldGame.Portada;
                    }
                    repositorio.Modificacion(juego);
                    TempData["Mensaje"] = "El Juego se modificó con éxito";
                    return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo Editar";
                    ViewBag.Creadores = repCreador.ObtenerTodos();
                    return View(juego);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Juegos/Delete/5
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

        // POST: Juegos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Juego juego)
        {
            try
            {
                repositorio.Baja(juego);
                TempData["Mensaje"] = "El Juego se eliminó con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["Mensaje"] = "El Juego está asociado a Noticias. Imposible Eliminar";
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}