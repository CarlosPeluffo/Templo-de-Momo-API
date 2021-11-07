namespace Templo_de_Momo.Models
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        Usuario ObtenerPorMail(string mail);
    }
}