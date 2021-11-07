using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Templo_de_Momo.Models;

namespace Templo_de_Momo.Controllers
{
    public class ComentariosController : Controller
    {
        private readonly IRepositorioComentario repositorio;
        private readonly IRepositorioUsuario repUsuario;
        private readonly IRepositorioNoticia repNoticia;
        private readonly IConfiguration configuration;
        public ComentariosController(IConfiguration configuration, 
                        IRepositorioComentario repositorio, IRepositorioNoticia repNoticia, IRepositorioUsuario repUsuario)
        {
            this.repositorio = repositorio;
            this.repNoticia = repNoticia;
            this.repUsuario = repUsuario;
            this.configuration = configuration;
        }
        // GET: Comentarios
        public ActionResult Index(int id)
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
                var lista = repositorio.ObtenerPorNoticia(id);
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }

        // GET: Comentarios/Details/5
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

        // GET: Comentarios/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Usuarios = repUsuario.ObtenerTodos();
                ViewBag.Noticias = repNoticia.ObtenerTodos();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Comentarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comentario comentario)
        {
            var idN = comentario.NoticiaId;
            try{
                if(ModelState.IsValid){
                    repositorio.Alta(comentario);
                    TempData["Id"] = comentario.Id;
                    return RedirectToAction("Index", new { id = idN});
                    }
                else{
                    ViewBag.Mensaje = "No se pudo cargar";
                    ViewBag.Usuarios = repUsuario.ObtenerTodos();
                    ViewBag.Noticias = repNoticia.ObtenerTodos();
                    return View(comentario);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction("Index", new { id = idN});
            }
        }

        // GET: Comentarios/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                ViewBag.Usuarios = repUsuario.ObtenerTodos();
                ViewBag.Noticias = repNoticia.ObtenerTodos();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Comentarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comentario comentario)
        {
            var idN = comentario.NoticiaId;
            try{
                if(ModelState.IsValid){
                    repositorio.Modificacion(comentario);
                    TempData["Mensaje"] = "El Comentario se modificó con éxito";
                    return RedirectToAction("Index", new { id = idN});
                    }
                else{
                    ViewBag.Mensaje = "No se pudo Editar";
                    ViewBag.Usuarios = repUsuario.ObtenerTodos();
                    ViewBag.Noticias = repNoticia.ObtenerTodos();
                    return View(comentario);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction("Index", new { id = idN});
            }
        }

        // GET: Comentarios/Delete/5
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

        // POST: Comentarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Comentario comentario)
        {
            try
            {
                repositorio.Baja(comentario);
                TempData["Mensaje"] = "Comentario se eliminó con éxito";
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