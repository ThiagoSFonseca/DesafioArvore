using DesafioArvore.Domain.Entidades;

namespace DesafioArvore.Contratos
{
    public class PessoaDomainService : IPessoaDomainService
    {
        public async Task<List<ArvoreGenealogica>> ObterArvoreAteONivelEspecificado(long idIndividuo, IEnumerable<Pessoa> arvoreGenealogica, int nivelMaximoArvore, int nivelAtualArvore = 0)
        {
            return await _IDomainService.ObterArvoreAteONivelEspecificado(idIndividuo, arvoreGenealogica, nivelMaximoArvore, nivelAtualArvore);
        }

        public double ObterPercentualDePessoasRepetidas(IEnumerable<Pessoa> pessoasPorRegiao)
        {
            return _IDomainService.ObterPercentualDePessoasRepetidas(pessoasPorRegiao);
        }
    }
}