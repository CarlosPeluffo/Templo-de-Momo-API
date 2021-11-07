using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Templo_de_Momo.Models
{
    public class Biblioteca
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaSeguido { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        public Usuario Usuario { get; set; }
        [Required]
        public int JuegoId { get; set; }
        [ForeignKey(nameof(JuegoId))]
        public Juego Juego { get; set; }
    }
}