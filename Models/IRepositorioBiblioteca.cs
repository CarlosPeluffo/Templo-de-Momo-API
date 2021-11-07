using System.Collections.Generic;

namespace Templo_de_Momo.Models
{
    public interface IRepositorioBiblioteca : IRepositorio<Biblioteca>
    {
        IList<Biblioteca> ObtenerPorUsuario(int id);
    }
}