using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Templo_de_Momo.Models
{
    public class Noticia
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Cuerpo { get; set; }
        [Required]
        public int JuegoId { get; set; }
        [ForeignKey(nameof(JuegoId))]
        public Juego Juego { get; set; }
        [Required]
        public int CreadorId { get; set; }
        [ForeignKey(nameof(CreadorId))]
        public Creador Creador { get; set; }
    }
}