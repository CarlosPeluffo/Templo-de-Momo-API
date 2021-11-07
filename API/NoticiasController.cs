using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Templo_de_Momo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Templo_de_Momo.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class NoticiasController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        public NoticiasController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            try
            {
                var usuario = User.Identity.Name;
                var lista = await context.noticias
                    .Include(x => x.Juego)
                    .Include(x => x.Creador)
                    .Where(x => x.Juego.Id == id && x.Creador.Mail == usuario)
                    .ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("Detalles/{id}")]
        public async Task<IActionResult> Detalles(int id){
            try
            {
                var usuario = User.Identity.Name;
                var entidad = await context.noticias
                    .Include(x => x.Creador)
                    .Where(x => x.Creador.Mail == usuario)
                    .SingleOrDefaultAsync(x => x.Id == id);
                return entidad != null ? Ok(entidad) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Noticia noticia){
            try
            {
                var usuario = User.Identity.Name;
                noticia.CreadorId = context.creadores.Single(x => x.Mail == usuario).Id;
                noticia.Fecha = DateTime.Now.Date;
                context.noticias.Add(noticia);
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(Detalles), new {id = noticia.Id}, noticia);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Noticia noticia){
            try
            {
                var usuario = User.Identity.Name;
                var OldNoticia = context.noticias
                    .AsNoTracking()
                    .Include(x => x.Creador)
                    .FirstOrDefault(x => x.Id == id && x.Creador.Mail == usuario);
                if(OldNoticia != null){
                    noticia.Id = OldNoticia.Id;
                    noticia.Fecha = OldNoticia.Fecha;
                    noticia.CreadorId = OldNoticia.CreadorId;
                    noticia.JuegoId = OldNoticia.JuegoId;
                    context.noticias.Update(noticia);
                    await context.SaveChangesAsync();
                    return Ok(noticia);
                }else{
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}