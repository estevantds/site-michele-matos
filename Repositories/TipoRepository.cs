using MiMatos.Context;
using MiMatos.Models;
using MiMatos.Repositories.Interfaces;

namespace MiMatos.Repositories
{
    public class TipoRepository : ITipoRepository
    {
        private readonly AppDbContext _context;

        public TipoRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Tipo> Tipos => _context.Tipos;
    }
}
