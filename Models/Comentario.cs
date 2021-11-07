using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Templo_de_Momo.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }
        [Required]
        public string Cuerpo { get; set; }
        public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        public Usuario Usuario { get; set; }
        public int NoticiaId { get; set; }
        [ForeignKey(nameof(NoticiaId))]
        public Noticia Noticia { get; set; }

    }
}