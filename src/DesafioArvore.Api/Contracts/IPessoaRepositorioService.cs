using DesafioArvore.Domain.Entidades;
using static DesafioArvore.Models.Enums;

namespace DesafioArvore.Contratos
{
    public interface IPessoaRepositorioService
    {
        Task<IReadOnlyList<Pessoa>> ObterTodosAsync();

        Task<IReadOnlyList<Pessoa>> ObterPorID(int id);

        Task<IReadOnlyList<Pessoa>> ObterPessoaPorFiltros(int? id, string? nome,
                                                               string? sobrenome, CorDaPele? cor, int? idPai, int? idMae,
                                                               RegiaoBrasil? regiaoNascimento, NivelEscolaridade? escolaridade);


        Task<IReadOnlyList<Pessoa>> ObterPessoasPorRegiao(RegiaoBrasil? regiaoNascimento);

        Task<IReadOnlyList<Pessoa>> ObterArvoreGenealogicaDoIndividuo(int id);


        Task<Pessoa> CadastrarPessoa(Pessoa pessoa);


        Task<List<Pessoa>> CadastrarPessoas(List<Pessoa> pessoas);


        Task<Pessoa> AtualizarCadastroPessoa(Pessoa pessoa);

        Task DeletarPessoa(int id);       
    }
}
