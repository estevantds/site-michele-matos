using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiMatos.Models;

namespace MiMatos.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Proprietario> Proprietarios { get; set; }
        public DbSet<Propriedade> Propriedades { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Bairro> Bairros { get; set; }
        public DbSet<Condominio> Condominios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
        public DbSet<Localidade> Localidades { get; set; }
    }
}
