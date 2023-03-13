using DesafioArvore.Interfaces;
using DesafioArvore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using static DesafioArvore.Models.Enums;
using DesafioArvore.Infraestrutura.Persistence;

namespace DesafioArvore.Infraestrutura.Repository
{
    public class PessoaRepository : IPessoaRepository
    {
        protected readonly PessoaContext _dbContext;

        public PessoaRepository(PessoaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Pessoa>> ObterTodosAsync()
        {
            return await _dbContext.Pessoas.ToListAsync();
        }

        public async Task<Pessoa> ObterPorID(int id)
        {
            return await _dbContext.Pessoas.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Pessoa>> ObterPessoaPorFiltros(int? id, string? nome,
                                                                        string? sobrenome, CorDaPele? cor, int? idPai, int? idMae,
                                                                        RegiaoBrasil? regiaoNascimento, NivelEscolaridade? escolaridade)
        {
            return await _dbContext.Pessoas.FromSqlRaw(@"SELECT * 
                                                            FROM Pessoas p
                                                            WHERE 
                                                            p.id    = ISNULL({0}, p.Id) AND 
                                                            (p.idPai IS NULL OR p.idPai = ISNULL({1}, p.idPai)) AND 
                                                            (p.idMae IS NULL OR p.idMae = ISNULL({2}, p.idMae)) AND 
                                                            (p.Cor IS NULL OR p.Cor   = ISNULL({3}, p.Cor)) AND 
                                                            (p.RegiaoNascimento IS NULL OR p.RegiaoNascimento   = ISNULL({4}, p.RegiaoNascimento)) AND 
                                                            (p.Escolaridade IS NULL OR p.Escolaridade   = ISNULL({5}, p.Escolaridade)) AND
                                                            p.Nome  = ISNULL({6}, p.Nome) AND 
                                                            p.Sobrenome = ISNULL({7}, p.Sobrenome) ",
                                                            id,
                                                            idPai,
                                                            idMae,
                                                            cor,
                                                            regiaoNascimento,
                                                            escolaridade,
                                                            string.IsNullOrEmpty(nome) ? null : string.Format("{0}", nome),
                                                            string.IsNullOrEmpty(sobrenome) ? null : string.Format("{0}", sobrenome)).ToListAsync();
        }

        public async Task<IEnumerable<Pessoa>> ObterPessoasPorRegiao(RegiaoBrasil? regiaoNascimento)
        {
            return await _dbContext.Pessoas.Where(x => x.RegiaoNascimento == regiaoNascimento).ToListAsync();
        }

        public async Task<IEnumerable<Pessoa>> ObterArvoreGenealogicaDoIndividuo(int id)
        {
            return await _dbContext.Pessoas.FromSqlRaw(@"WITH Genealogia AS (
                                                                                SELECT *
                                                                                FROM Pessoas
                                                                                WHERE Id = {0}
                                                                                UNION ALL
                                                                                SELECT p.*
                                                                                FROM Pessoas p
                                                                                JOIN Genealogia g ON (p.Id = g.IdPai OR p.Id = g.IdMae)
                                                                             )
                                                                             SELECT * FROM Genealogia;", id).ToListAsync();
        }

        public async Task<Pessoa> CadastrarPessoa(Pessoa pessoa)
        {
            _dbContext.Add(pessoa);
            await _dbContext.SaveChangesAsync();
            return pessoa;
        }

        public async Task<IEnumerable<Pessoa>> CadastrarPessoas(IEnumerable<Pessoa> pessoas)
        {
            await _dbContext.AddRangeAsync(pessoas);
            await _dbContext.SaveChangesAsync();
            return pessoas;
        }

        public async Task<Pessoa> AtualizarCadastroPessoa(Pessoa pessoa)
        {
            _dbContext.Entry(pessoa).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return pessoa;
        }

        public async Task DeletarPessoa(int id)
        {
            _dbContext.Pessoas.Remove(new Pessoa() { Id = id });
            await _dbContext.SaveChangesAsync();
        }

       
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
