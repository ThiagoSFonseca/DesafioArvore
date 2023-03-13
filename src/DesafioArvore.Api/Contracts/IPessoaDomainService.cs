using DesafioArvore.Domain.Entidades;

namespace DesafioArvore.Contratos
{
    public interface IPessoaDomainService
    {
        Task<List<ArvoreGenealogica>> ObterArvoreAteONivelEspecificado(long idIndividuo, IEnumerable<Pessoa> arvoreGenealogica, int nivelMaximoArvore, int nivelAtualArvore = 0);

        double ObterPercentualDePessoasRepetidas(IEnumerable<Pessoa> pessoasPorRegiao);
    }
}