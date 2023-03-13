using DesafioArvore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioArvore.Infraestrutura.Persistence
{
    public class PessoaContext : DbContext
    {
        public PessoaContext()
        { }

        public PessoaContext(DbContextOptions<PessoaContext> opcoes) : base(opcoes)
        { }

        public DbSet<Pessoa> Pessoas { get; set; }

    }
}
