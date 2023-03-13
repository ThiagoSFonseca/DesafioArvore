using DesafioArvore.Interfaces;
using DesafioArvore.Domain.Models;
using DesafioArvore.Models;
using static DesafioArvore.Models.Enums;

namespace DesafioArvore.Domain.Services
{
    public class PessoaDomainService : IPessoaDomainService
    {
        IPessoaRepository _IPessoaRepository;
        public PessoaDomainService(IPessoaRepository iPessoaRepository)
        {
            _IPessoaRepository = iPessoaRepository;
        }

        public async Task<Pessoa> AtualizarCadastroPessoa(Pessoa pessoa)
        {
            return await _IPessoaRepository.AtualizarCadastroPessoa(pessoa);
        }

        public async Task<Pessoa> CadastrarPessoa(Pessoa pessoa)
        {
            return await _IPessoaRepository.CadastrarPessoa(pessoa);
        }

        public async Task<IEnumerable<Pessoa>> CadastrarPessoas(IEnumerable<Pessoa> pessoas)
        {
            return await _IPessoaRepository.CadastrarPessoas(pessoas);
        }

        public async Task DeletarPessoa(int id)
        {
            await _IPessoaRepository.DeletarPessoa(id);
        }

        public async Task<IEnumerable<Pessoa>> ObterPessoaPorFiltros(int? id, string? nome, string? sobrenome, CorDaPele? cor, int? idPai, int? idMae, RegiaoBrasil? regiaoNascimento, NivelEscolaridade? escolaridade)
        {
            return await _IPessoaRepository.ObterPessoaPorFiltros(id, nome, sobrenome, cor, idPai, idMae, regiaoNascimento, escolaridade);
        }

        public async Task<Pessoa> ObterPorID(int id)
        {
            return await _IPessoaRepository.ObterPorID(id);
        }

        public async Task<IEnumerable<Pessoa>> ObterTodosAsync()
        {            
            return await _IPessoaRepository.ObterTodosAsync();
        }

        public async Task<IEnumerable<Pessoa>> ObterArvoreGenealogicaDoIndividuo(int id)
        {
            return await _IPessoaRepository.ObterArvoreGenealogicaDoIndividuo(id);
        }

        public async Task<IEnumerable<ArvoreGenealogica>> ObterArvoreAteONivelEspecificado(long idIndividuo, IEnumerable<Pessoa> arvoreGenealogica, int nivelMaximoArvore, int nivelAtualArvore = 0)
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

        public async Task<IEnumerable<Pessoa>> ObterPessoasPorRegiao(RegiaoBrasil? regiaoNascimento)
        {
            return await _IPessoaRepository.ObterPessoasPorRegiao(regiaoNascimento);
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}