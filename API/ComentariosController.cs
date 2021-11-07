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
    public class ComentariosController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        public ComentariosController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            try
            {
                var usuario = User.Identity.Name;
                var lista = await context.comentarios
                    .Include(x => x.Noticia)
                    .Include(x => x.Usuario)
                    .Where(x => x.Noticia.Id == id && x.Noticia.Creador.Mail == usuario)
                    .ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}