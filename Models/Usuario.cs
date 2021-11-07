using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Templo_de_Momo.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [Column("nick_name")]
        public string NickName { get; set; }
        [Required, EmailAddress]
        public string Mail { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public string Avatar { get; set; }
        [NotMapped]
        public IFormFile AvatarFile { get; set; }
    }
}