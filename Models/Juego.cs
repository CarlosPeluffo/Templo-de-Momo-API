using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace Templo_de_Momo.Models
{
    public class Juego
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        public string Portada { get; set; }
        [NotMapped]
        public IFormFile PortadaFile { get; set; }
        [NotMapped]
        public string PortadaMovil { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public string Requisitos { get; set; }
        public double Precio { get; set; }
        [Required]
        public int CreadorId { get; set; }
        [ForeignKey(nameof(CreadorId))]
        public Creador Creador { get; set; }
    }
}