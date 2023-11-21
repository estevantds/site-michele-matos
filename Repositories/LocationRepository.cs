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

        public IEnumerable<Localidade> Localidades => _context.Localidades;
    }
}
