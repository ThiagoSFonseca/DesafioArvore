using DesafioArvore.Domain.Models;
using static DesafioArvore.Models.Enums;

namespace DesafioArvore.Interfaces
{
    public interface IPessoaDomainService : IDisposable 
    {
        Task<IEnumerable<Pessoa>> ObterTodosAsync();

        Task<Pessoa> ObterPorID(int id);

        Task<IEnumerable<Pessoa>> ObterPessoaPorFiltros(int? id, string? nome,
                                                               string? sobrenome, CorDaPele? cor, int? idPai, int? idMae,
                                                               RegiaoBrasil? regiaoNascimento, NivelEscolaridade? escolaridade);


        Task<IEnumerable<Pessoa>> ObterPessoasPorRegiao(RegiaoBrasil? regiaoNascimento);

        Task<IEnumerable<Pessoa>> ObterArvoreGenealogicaDoIndividuo(int id);


        Task<Pessoa> CadastrarPessoa(Pessoa pessoa);


        Task<IEnumerable<Pessoa>> CadastrarPessoas(IEnumerable<Pessoa> pessoas);


        Task<Pessoa> AtualizarCadastroPessoa(Pessoa pessoa);

        Task DeletarPessoa(int id);

        Task<IEnumerable<ArvoreGenealogica>> ObterArvoreAteONivelEspecificado(long idIndividuo, IEnumerable<Pessoa> arvoreGenealogica, int nivelMaximoArvore, int nivelAtualArvore = 0);

        double ObterPercentualDePessoasRepetidas(IEnumerable<Pessoa> pessoasPorRegiao);
    }
}