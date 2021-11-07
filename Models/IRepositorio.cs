using System.Collections.Generic;

namespace Templo_de_Momo.Models
{
    public interface IRepositorio<T>
    {
        int Alta(T t);
        int Baja(T t);
        int Modificacion(T t);
        IList<T> ObtenerTodos();
        T ObtenerPorId(int id);
    }
}