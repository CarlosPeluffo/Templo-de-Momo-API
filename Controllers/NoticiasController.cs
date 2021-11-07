using System;
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
    public class NoticiasController : Controller
    {
        private readonly IRepositorioNoticia repositorio;
        private readonly IRepositorioCreador repCreador;
        private readonly IRepositorioJuego repJuego;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        public NoticiasController(IConfiguration configuration, IWebHostEnvironment environment, 
                        IRepositorioNoticia repositorio, IRepositorioJuego repJuego, IRepositorioCreador repCreador)
        {
            this.repositorio = repositorio;
            this.repCreador = repCreador;
            this.repJuego = repJuego;
            this.configuration = configuration;
            this.environment = environment; 
        }
        // GET: Noticias
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

        // GET: Noticias/Details/5
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

        // GET: Noticias/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Creadores = repCreador.ObtenerTodos();
                ViewBag.Juegos = repJuego.ObtenerTodos();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Noticias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Noticia noticia)
        {
            try{
                if(ModelState.IsValid){
                        repositorio.Alta(noticia);
                        TempData["Id"] = noticia.Id;
                        return RedirectToAction(nameof(Index));
                    }
                else{
                    ViewBag.Mensaje = "No se pudo cargar";
                    ViewBag.Creadores = repCreador.ObtenerTodos();
                    ViewBag.Juegos = repJuego.ObtenerTodos();
                    return View(noticia);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Noticias/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                ViewBag.Creadores = repCreador.ObtenerTodos();
                ViewBag.Juegos = repJuego.ObtenerTodos();
                return View(entidad);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Noticias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Noticia noticia)
        {
            try
            {
                if(ModelState.IsValid){
                    repositorio.Modificacion(noticia);
                    TempData["Mensaje"] = "La Noticia se modificó con éxito";
                    return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo Editar";
                    ViewBag.Creadores = repCreador.ObtenerTodos();
                    ViewBag.Juegos = repJuego.ObtenerTodos();
                    return View(noticia);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Noticias/Delete/5
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

        // POST: Noticias/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Noticia noticia)
        {
            try
            {
                repositorio.Baja(noticia);
                TempData["Mensaje"] = "Noticia se eliminó con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}