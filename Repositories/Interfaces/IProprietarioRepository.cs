using MiMatos.Models;

namespace MiMatos.Repositories.Interfaces
{
    public interface IProprietarioRepository
    {
        IEnumerable<Proprietario> Proprietarios { get; }
    }
}
