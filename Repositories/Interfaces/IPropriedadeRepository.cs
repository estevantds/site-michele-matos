using MiMatos.Models;

namespace MiMatos.Repositories.Interfaces
{
    public interface IPropriedadeRepository
    {
        IEnumerable<Propriedade> Propriedades { get; }
        IEnumerable<Propriedade> PropriedadesDisponiveis { get; }
        IEnumerable<Propriedade> PropriedadesDisponiveisRecemAdicionadas();
        Propriedade GetPropriedadeByCodigo(string codigo);
    }
}
