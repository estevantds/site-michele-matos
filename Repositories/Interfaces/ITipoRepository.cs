using MiMatos.Models;

namespace MiMatos.Repositories.Interfaces
{
    public interface ITipoRepository
    {
        IEnumerable<Tipo> Tipos { get; }
    }
}
