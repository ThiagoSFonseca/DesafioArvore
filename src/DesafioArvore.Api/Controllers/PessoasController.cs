using Microsoft.AspNetCore.Mvc;
using static DesafioArvore.Models.Enums;
using DesafioArvore.Domain.Models;
using DesafioArvore.Interfaces;
using DesafioArvore.Domain.Services;
using System.Data;
using Microsoft.SqlServer.Server;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DesafioArvore.Controllers
{

    [Route("api/")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaDomainService _pessoaDomainService;

        public PessoasController(IPessoaDomainService pessoaDomainService)
          {
            _pessoaDomainService = pessoaDomainService;
          }

        #region GET

        [HttpGet("pessoas")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> ObterPessoaPorFiltros(int? id, string? nome, 
                                                                        string? sobrenome, CorDaPele? cor, int? idPai, int? idMae,
                                                                        RegiaoBrasil? regiaoNascimento, NivelEscolaridade? escolaridade)
        {
            var retorno = await _pessoaDomainService.ObterPessoaPorFiltros(id, nome, sobrenome, cor, idPai, idMae, regiaoNascimento, escolaridade);  

            if (!retorno.Any()) 
            {
                return NotFound("Não foram encontrados registros para os parâmetros informados");
            }

            return Ok(retorno);
        }
        
        [Route("pessoas/{regiao}/percentual")]
        [HttpGet]
        public async Task<ActionResult<Tuple<string,double>>> ObterPercentualPessoasComMesmoNomePorRegiao(RegiaoBrasil regiao)
        {
            var pessoasPorRegiao = await _pessoaDomainService.ObterPessoasPorRegiao(regiao);

            double percentualRepetidos = _pessoaDomainService.ObterPercentualDePessoasRepetidas(pessoasPorRegiao);
            string retornoFormatado = string.Format(@"O Percentual de nomes repetidos na região {0} é {1}%", regiao.ToString(), percentualRepetidos.ToString("F2"));
            Tuple<string, double> retorno = new Tuple<string, double>(retornoFormatado, percentualRepetidos);

            return Ok(retorno);
                          
        }

        [Route("pessoas/{id}/arvore-genealogica/{nivelMaximoArvore}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> ObterArvoreGenealogicaPorNivel(int id, int nivelMaximoArvore)
        {
            var arvoreGenealogica = await _pessoaDomainService.ObterArvoreGenealogicaDoIndividuo(id);

            if (!arvoreGenealogica.Any())
                return NotFound("Pessoa informada não encontrada");

            var arvoreNivelEspecificado = await _pessoaDomainService.ObterArvoreAteONivelEspecificado(id, arvoreGenealogica, nivelMaximoArvore);
            
            if (arvoreNivelEspecificado != null)
                return Ok(arvoreNivelEspecificado.OrderBy(x => x.Nivel).ToList());
            else
                return StatusCode(StatusCodes.Status500InternalServerError, arvoreNivelEspecificado);
        }
       

        [Route("pessoas/cores")]
        [HttpGet()]
        public ActionResult<IEnumerable<string>> ObterCoresPele()
        {
           var coresDaPele = Enum.GetNames(typeof(CorDaPele)).ToList();
            return coresDaPele;
        }

        [Route("pessoas/regioes")]
        [HttpGet()]
        public ActionResult<IEnumerable<string>> ObterRegioesBrasil()
        {
            var regioesBrasil = Enum.GetNames(typeof(RegiaoBrasil)).ToList();
            return regioesBrasil;
        }

        [Route("pessoas/escolaridades")]
        [HttpGet()]
        public ActionResult<IEnumerable<string>> ObterNiveisEscolaridade()
        {
            var niveisEscolaridade = Enum.GetNames(typeof(NivelEscolaridade)).ToList();
            return niveisEscolaridade;
        }

        [Route("pessoas/ping")]
        [HttpGet()]
        public ActionResult<string> HealthCheck()
        {
            return Ok("pong");
        }

        #endregion

        #region POST

        [Route("pessoas")]
        [HttpPost]
        public async Task<ActionResult> CadastrarPessoas([FromBody] List<Pessoa> pessoas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _pessoaDomainService.CadastrarPessoas(pessoas);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao cadastrar pessoas: {ex.Message}");
            }
        }

        #endregion

        #region PUT

        [Route("pessoas")]
        [HttpPut()]
        public async Task<IActionResult> AtualizarCadastroPessoa(int id, [FromBody] Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return BadRequest();
            }

            try
            {
                await _pessoaDomainService.AtualizarCadastroPessoa(pessoa);
            }
            catch (DBConcurrencyException)
            {
                var pessoaExiste = await _pessoaDomainService.ObterPorID(pessoa.Id);
                if (pessoaExiste == null || pessoa.Id != id)
                {
                    return NotFound("Pessoa informada não existe");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        #endregion
        
        #region DELETE

        [Route("pessoas")]
        [HttpDelete()]
        public async Task<IActionResult> DeletarPessoa(int id)
        {
            try
            {
                await _pessoaDomainService.DeletarPessoa(id);
            }
            catch (DBConcurrencyException)
            {
                var pessoaExiste = await _pessoaDomainService.ObterPorID(id);
                if (pessoaExiste == null || pessoaExiste.Id != id)
                {
                    return NotFound("Pessoa informada não existe");
                }
                else
                {
                    throw;
                }
            }
            
            return NoContent();
        }
        
        #endregion
    }
}
