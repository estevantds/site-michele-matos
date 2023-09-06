using MiMatos.Context;
using MiMatos.Models;
using MiMatos.Repositories.Interfaces;

namespace MiMatos.Repositories
{
    public class ProprietarioRepository : IProprietarioRepository
    {
        private readonly AppDbContext _context;

        public ProprietarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Proprietario> Proprietarios => _context.Proprietarios;
    }
}
