using Proyecto_WEB.Models;

namespace Proyecto_WEB.Servicios
{
    public interface IMetodosComunes
    {
        string Encrypt(string texto);
        List<Miembro> ConsultarMembresiaMiembro();
        List<Carrito> ConsultarCarrito();
    }
}
