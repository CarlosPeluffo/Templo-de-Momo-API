using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Templo_de_Momo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Templo_de_Momo.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CreadoresController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        public CreadoresController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        [HttpGet]
        public async Task<ActionResult<Creador>> Get(){
            try
            {
                var usuario = User.Identity.Name;
                return await context.creadores.SingleOrDefaultAsync(x => x.Mail == usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login ([FromForm] LoginView loginView){
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var creador = await context.creadores.FirstOrDefaultAsync(x => x.Mail == loginView.Usuario);
                if( creador == null || creador.Password != hashed)
                {
                    return BadRequest("Usuario o clave incorrectos");
                }
                else
                {
                    var key = new SymmetricSecurityKey(
                        System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, creador.Mail),
                        new Claim("FullName", creador.Nombre + " " + creador.Apellido),
                        new Claim(ClaimTypes.Role, "Creador"),
                    };
                    var token = new JwtSecurityToken(
                        issuer: configuration["TokenAuthentication:Issuer"],
                        audience: configuration["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Creador entidad){
            try
            {
                var usuario = User.Identity.Name;
                var creador = context.creadores.AsNoTracking().Single(x => x.Mail == usuario);
                if (entidad.Mail == usuario)
                {
                    entidad.Id = creador.Id;
                    context.creadores.Update(entidad);
                    await context.SaveChangesAsync();
                    return Ok(entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}