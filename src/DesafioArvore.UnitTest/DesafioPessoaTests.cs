using DesafioArvore.Controllers;
using DesafioArvore.Domain.Models;
using DesafioArvore.Domain.Services;
using DesafioArvore.Infraestrutura.Repository;
using DesafioArvore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static DesafioArvore.Models.Enums;

namespace DesafioArvore.UnitTest
{
    public class DesafioPessoaTests
    {
        private readonly Mock<IPessoaDomainService> _domainServiceMock;
        private readonly PessoaController _pessoaController;

        public DesafioPessoaTests()
        {
            _domainServiceMock = new Mock<IPessoaDomainService>();
            _pessoaController = new PessoaController(_domainServiceMock.Object);
        }

        [Fact]
        public async Task Quando_ObterTodasPessoas_Chamado_Retorna_200OKAsync()
        {
            var pessoas = new List<Pessoa>();
            pessoas.Add(new Pessoa() { Id = 1 });
            pessoas.Add(new Pessoa() { Id = 2 });

            // Arrange            
            _domainServiceMock.Setup(x => x.ObterTodosAsync())
                .ReturnsAsync(pessoas);

            // Act
            var result = await _pessoaController.ObterTodasPessoas();
            // Assert            
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, ((OkObjectResult)result.Result).StatusCode);
            Assert.IsAssignableFrom<List<Pessoa>>(((OkObjectResult)result.Result).Value);
            Assert.NotNull((List<Pessoa>)((OkObjectResult)result.Result).Value);
            Assert.True(((List<Pessoa>)((OkObjectResult)result.Result).Value).Any());
        }

        [Fact]
        public async Task Quando_ObterPessoaPorID_Chamado_Retorna_200OKAsync()
        {
            Pessoa pessoa = new Pessoa() { Id = 1 };
            // Arrange            
            _domainServiceMock.Setup(x => x.ObterPorID(1))
                .ReturnsAsync(pessoa);

            // Act
            var result = await _pessoaController.ObterPessoaPorID(1);


            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, ((OkObjectResult)result.Result).StatusCode);
            Assert.IsAssignableFrom<Pessoa>(((OkObjectResult)result.Result).Value);
            Assert.NotNull((Pessoa)((OkObjectResult)result.Result).Value);
            Assert.True(((Pessoa)((OkObjectResult)result.Result).Value).Id == 1);
        }

        [Fact]
        public async Task Quando_ObterPessoaPorID_Chamado_Retorna_404NotFound()
        {
            Pessoa pessoa = new Pessoa() { Id = 1 };
            // Arrange            
            _domainServiceMock.Setup(x => x.ObterPorID(1))
                .ReturnsAsync(pessoa);

            // Act
            var result = await _pessoaController.ObterPessoaPorID(99999);

            // Assert            
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, ((NotFoundObjectResult)result.Result).StatusCode);
        }

        [Fact]
        public async Task Quando_ObterPessoaPorFiltros_Chamado_Retorna_200OKAsync()
        {
            //Concatenação de filtros para extração de dados. Ex: Quero o número de indivíduos Negros, com
            //formação superior e com Nome João
            var pessoas = new List<Pessoa>();
            pessoas.Add(new Pessoa() { Id = 1, Nome = "João", Cor = CorDaPele.Negra, Escolaridade = NivelEscolaridade.EnsinoSuperior });

            // Arrange            
            _domainServiceMock.Setup(x => x.ObterPessoaPorFiltros(null, "João", null, CorDaPele.Negra, 
                                                                  null, null, null, NivelEscolaridade.EnsinoSuperior))
                .ReturnsAsync(pessoas);

            // Act
            var result = await _pessoaController.ObterPessoaPorFiltros(null, "João", null, CorDaPele.Negra,
                                                                       null, null, null, NivelEscolaridade.EnsinoSuperior);

            // Assert            
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, ((OkObjectResult)result.Result).StatusCode);
            Assert.IsAssignableFrom<List<Pessoa>>(((OkObjectResult)result.Result).Value);
            Assert.NotNull((List<Pessoa>)((OkObjectResult)result.Result).Value);
            Assert.True(((List<Pessoa>)((OkObjectResult)result.Result).Value).Any());
        }

        [Fact]
        public async Task Quando_ObterPessoaPorFiltros_Chamado_Retorna_404NotFound()
        {
            var pessoas = new List<Pessoa>();

            // Act
            var result = await _pessoaController.ObterPessoaPorFiltros(null, "João", null, CorDaPele.Negra,
                                                                       null, null, null, NivelEscolaridade.EnsinoSuperior);


            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, ((NotFoundObjectResult)result.Result).StatusCode);
        }

        [Fact]
        public async Task Quando_ObterPercentualPessoasComMesmoNomePorRegiao_Chamado_Retorna_Percentual100_200OKAsync()
        {
            var pessoasPorRegiao = new List<Pessoa>();
            pessoasPorRegiao.Add(new Pessoa() { Id = 1, Nome = "João", RegiaoNascimento = RegiaoBrasil.Nordeste });
            pessoasPorRegiao.Add(new Pessoa() { Id = 2, Nome = "João", RegiaoNascimento = RegiaoBrasil.Nordeste });
            double percentualPessoasRepetidas = 100;

            // Arrange            
            _domainServiceMock.Setup(x => x.ObterPessoasPorRegiao(RegiaoBrasil.Nordeste))
                .ReturnsAsync(pessoasPorRegiao);

            _domainServiceMock.Setup(x => x.ObterPercentualDePessoasRepetidas(pessoasPorRegiao))
                .Returns(percentualPessoasRepetidas);

            // Act
            var result = await _pessoaController.ObterPercentualPessoasComMesmoNomePorRegiao(RegiaoBrasil.Nordeste);

            // Assert            
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, ((OkObjectResult)result.Result).StatusCode);
            Assert.IsAssignableFrom<Tuple<string, double>>(((OkObjectResult)result.Result).Value);
            Assert.NotNull((Tuple<string, double>)((OkObjectResult)result.Result).Value);
            Assert.True(((Tuple<string, double>)((OkObjectResult)result.Result).Value).Item2 == 100);
        }

        [Fact]
        public async Task Quando_ObterArvoreAteONivelEspecificado_Chamado_Retorna_NivelEspecificado_200OKAsync()
        {
            int idIndividuo = 4;
            var arvoreGenealogicaDoIndividuo = new List<Pessoa>();
            arvoreGenealogicaDoIndividuo.Add(new Pessoa() { Id = 1, Nome = "João" });
            arvoreGenealogicaDoIndividuo.Add(new Pessoa() { Id = 2, Nome = "Maria" });
            arvoreGenealogicaDoIndividuo.Add(new Pessoa() { Id = 3, Nome = "José", IdMae = 2, IdPai = 1 });
            arvoreGenealogicaDoIndividuo.Add(new Pessoa() { Id = idIndividuo, Nome = "Beatriz", IdPai = 3 });
            int nivelArvoreEspecificado = 1;
            

            // Arrange            
            _domainServiceMock.Setup(x => x.ObterArvoreGenealogicaDoIndividuo(idIndividuo))
                .ReturnsAsync(arvoreGenealogicaDoIndividuo);

            var iPessoaRepository = new Mock<IPessoaRepository>();
            PessoaDomainService domainService = new PessoaDomainService(iPessoaRepository.Object);
            var arvoreGenealogicaComNivelEspecificado = domainService.ObterArvoreAteONivelEspecificado(4, arvoreGenealogicaDoIndividuo, nivelArvoreEspecificado);

            _domainServiceMock.Setup(x => x.ObterArvoreAteONivelEspecificado(idIndividuo, arvoreGenealogicaDoIndividuo, nivelArvoreEspecificado, 0))
                .Returns(arvoreGenealogicaComNivelEspecificado);

            // Act
            var result = await _pessoaController.ObterArvoreGenealogicaPorNivel(4, nivelArvoreEspecificado);


            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, ((OkObjectResult)result.Result).StatusCode);
            Assert.IsAssignableFrom<List<ArvoreGenealogica>>(((OkObjectResult)result.Result).Value);
            Assert.True(((List<ArvoreGenealogica>)((OkObjectResult)result.Result).Value).Any());
            Assert.True(!arvoreGenealogicaComNivelEspecificado.ConfigureAwait(false).
                                                              GetAwaiter().GetResult().ToList().Exists(x => x.Nivel > nivelArvoreEspecificado));
         
        }

        [Fact]
        public async Task Quando_CadastrarPessoaAsync_Chamado_Retorna_200OKAsync()
        {
            // Arrange
            var pessoa = new Pessoa();
            pessoa.Nome = "João";
            pessoa.Sobrenome = "Gomes";

            var result = await _pessoaController.CadastrarPessoa(pessoa);

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
            Assert.Equal(200, ((OkResult)result).StatusCode);
        }

        [Fact]
        public async Task Quando_AtualizarPessoaAsync_Retorna_204NoContent()
        {
            var pessoa = new Pessoa();
            pessoa.Id = 1;
            pessoa.Nome = "João";
            pessoa.Sobrenome = "Gomes";

            var result = await _pessoaController.AtualizarCadastroPessoa(1 ,pessoa);

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, ((NoContentResult)result).StatusCode);

        }

        [Fact]
        public async Task Quando_AtualizarPessoaAsync_Chamado_Retorna_400BadRequest()
        {
            var pessoa = new Pessoa();
            pessoa.Id = 1;
            pessoa.Nome = "João";
            pessoa.Sobrenome = "Gomes";

            var result = await _pessoaController.AtualizarCadastroPessoa(2, pessoa);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, ((BadRequestResult)result).StatusCode);

        }

        [Fact]
        public async Task Quando_DeletarPessoaAsync_Chamado_Retorna_204NoContent()
        {

            var result = await _pessoaController.DeletarPessoa(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, ((NoContentResult)result).StatusCode);

        }
    }
}