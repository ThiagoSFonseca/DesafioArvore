using DesafioArvore.Domain;
using DesafioArvore.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using static DesafioArvore.Models.Enums;

namespace DesafioArvore.Contratos
{
    public class PessoaRepositorioService : IPessoaRepositorioService
    {
        IPessoaRepositorioService _IPessoaService;
        public PessoaRepositorioService(IPessoaRepositorioService iPessoaService)
        {
            _IPessoaService = iPessoaService;
        }

        public Task<Pessoa> AtualizarCadastroPessoa(Pessoa pessoa)
        {
            return _IPessoaService.AtualizarCadastroPessoa(pessoa);
        }

        public Task<Pessoa> CadastrarPessoa(Pessoa pessoa)
        {
            return _IPessoaService.CadastrarPessoa(pessoa);
        }

        public Task<List<Pessoa>> CadastrarPessoas(List<Pessoa> pessoas)
        {
            return _IPessoaService.CadastrarPessoas(pessoas);
        }

        public Task DeletarPessoa(int id)
        {
            return _IPessoaService.DeletarPessoa(id);
        }

        public Task<IReadOnlyList<Pessoa>> ObterArvoreGenealogicaDoIndividuo(int id)
        {
            return _IPessoaService.ObterArvoreGenealogicaDoIndividuo(id);
        }

        public Task<IReadOnlyList<Pessoa>> ObterPessoaPorFiltros(int? id, string? nome, string? sobrenome, CorDaPele? cor, int? idPai, int? idMae, RegiaoBrasil? regiaoNascimento, NivelEscolaridade? escolaridade)
        {
            return _IPessoaService.ObterPessoaPorFiltros(id, nome, sobrenome, cor, idPai, idMae, regiaoNascimento, escolaridade);
        }

        public Task<IReadOnlyList<Pessoa>> ObterPessoasPorRegiao(RegiaoBrasil? regiaoNascimento)
        {
            return _IPessoaService.ObterPessoasPorRegiao(regiaoNascimento);
        }

        public Task<IReadOnlyList<Pessoa>> ObterPorID(int id)
        {
            return _IPessoaService.ObterPorID(id);
        }

        public Task<IReadOnlyList<Pessoa>> ObterTodosAsync()
        {
            throw new Exception("teste status code");
            //return _IPessoaService.ObterTodosAsync();
        }
    }
}
