namespace Templo_de_Momo.Models
{
    public interface IRepositorioCreador : IRepositorio<Creador>
    {
        Creador ObtenerPorMail(string mail);
    }
}