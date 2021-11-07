using System.Collections.Generic;

namespace Templo_de_Momo.Models
{
    public interface IRepositorioComentario : IRepositorio<Comentario>
    {
        IList<Comentario> ObtenerPorNoticia(int id);
    }
}