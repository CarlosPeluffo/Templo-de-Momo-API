using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Templo_de_Momo.Controllers
{
    public class BibliotecasController : Controller
    {
        // GET: Bibliotecas
        public ActionResult Index()
        {
            return View();
        }

        // GET: Bibliotecas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Bibliotecas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bibliotecas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Bibliotecas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Bibliotecas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Bibliotecas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Bibliotecas/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}