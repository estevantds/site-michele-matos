using MiMatos.Models;

namespace MiMatos.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        IEnumerable<Estado> Estados { get; }
        IEnumerable<Cidade> Cidades { get; }
        IEnumerable<Bairro> Bairros { get; }
        IEnumerable<Condominio> Condominios { get; }
    }
}
