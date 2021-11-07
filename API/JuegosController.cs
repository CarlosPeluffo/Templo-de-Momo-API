using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Templo_de_Momo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Templo_de_Momo.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class JuegosController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        public JuegosController(DataContext context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
            this.environment = environment;
        }
        [HttpGet]
        public async Task<IActionResult> Get(){
            try
            {
                var usuario = User.Identity.Name;
                var lista = await context.juegos
                    .Include(x => x.Creador)
                    .Where(x => x.Creador.Mail == usuario).ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            try
            {
                var usuario = User.Identity.Name;
                var entidad = await context.juegos
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
        public async Task<IActionResult> Post([FromBody] Juego juego){
            try
            {
                var random = new Random();
                var num = random.Next(0, 100000);
                var usuario = User.Identity.Name;
                if(juego.PortadaMovil != null){
                    var stream = new MemoryStream(Convert.FromBase64String(juego.PortadaMovil));
                    IFormFile imagen = new FormFile(stream, 0, stream.Length, "juego", ".jpg");
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "UsersFiles");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                        string fileName = "portada_" + num + Path.GetExtension(imagen.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        juego.Portada = Path.Combine("/UsersFiles", fileName);
                        using (FileStream streamF = new FileStream(pathCompleto, FileMode.Create)){
                        imagen.CopyTo(streamF);
                    }
                    juego.CreadorId = context.creadores.Single(x => x.Mail == usuario).Id;
                    context.juegos.Add(juego);
                    await context.SaveChangesAsync();
                    return Ok(juego);
                    //return CreatedAtAction(nameof(Get), new {id = jeugo.Id}, juego);
                }
                else
                {
                    return BadRequest("No entra al if");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}