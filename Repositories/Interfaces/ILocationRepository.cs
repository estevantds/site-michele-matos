using MiMatos.Models;

namespace MiMatos.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        IEnumerable<Localidade> Localidades { get; }
    }
}
