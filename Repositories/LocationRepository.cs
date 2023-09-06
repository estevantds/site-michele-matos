using MiMatos.Context;
using MiMatos.Models;
using MiMatos.Repositories.Interfaces;

namespace MiMatos.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _context;

        public LocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Estado> Estados => _context.Estados;

        public IEnumerable<Cidade> Cidades => _context.Cidades;

        public IEnumerable<Bairro> Bairros => _context.Bairros;

        public IEnumerable<Condominio> Condominios => _context.Condominios;
    }
}
