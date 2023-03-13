using DesafioArvore.Domain.Entidades;
using DesafioArvore.Models;

namespace DesafioArvore.Domain
{
    public class PessoaDomain
    {
        public async Task<List<ArvoreGenealogica>> ObterArvoreAteONivelEspecificado(long idIndividuo, IEnumerable<Pessoa> arvoreGenealogica, int nivelMaximoArvore, int nivelAtualArvore = 0)
        {
            if (nivelAtualArvore > nivelMaximoArvore)
                return new List<ArvoreGenealogica>();

            List<ArvoreGenealogica> familia = new List<ArvoreGenealogica>();
            var pessoa = arvoreGenealogica.Where(x => x.Id == idIndividuo).SingleOrDefault();
            
            if (pessoa != null)
            {
                ArvoreGenealogica result = new ArvoreGenealogica(pessoa);
                var pai = arvoreGenealogica.Where(x => x.Id == pessoa.IdPai).SingleOrDefault();
                if (pai != null)
                    familia.AddRange(await ObterArvoreAteONivelEspecificado(pai.Id, arvoreGenealogica, nivelMaximoArvore, nivelAtualArvore + 1));

                var mae = arvoreGenealogica.Where(x => x.Id == pessoa.IdMae).SingleOrDefault();
                if (mae != null)
                    familia.AddRange(await ObterArvoreAteONivelEspecificado(mae.Id, arvoreGenealogica, nivelMaximoArvore, nivelAtualArvore + 1));

                result.Nivel = nivelAtualArvore;
                familia.Add(result);
            }

            return familia;
        }

        public double ObterPercentualDePessoasRepetidas(IEnumerable<Pessoa> pessoasPorRegiao)
        {
            var qtdePessoasPorRegiao = pessoasPorRegiao.Count();
            if (qtdePessoasPorRegiao == 0)
                return 0;

            Dictionary<string, int> nomesRepetidos = new Dictionary<string, int>();

            foreach (var item in pessoasPorRegiao)
            {
                if (!string.IsNullOrEmpty(item.Nome) && !nomesRepetidos.ContainsKey(item.Nome))
                    nomesRepetidos[item.Nome] = 0;

                nomesRepetidos[item.Nome]++;
            }

            int qtdRepetido = 0;

            foreach (int nomeRepetido in nomesRepetidos.Values)
            {
                if (nomeRepetido > 1)
                    qtdRepetido += nomeRepetido;
            }

            return (double)qtdRepetido / qtdePessoasPorRegiao * 100;
        }

    }
}
