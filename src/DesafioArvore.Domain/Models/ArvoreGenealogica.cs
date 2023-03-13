using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioArvore.Domain.Models
{
    public class ArvoreGenealogica : Pessoa
    {
        public ArvoreGenealogica(Pessoa pessoa)
        {
            this.Id = pessoa.Id;
            this.Nome = pessoa.Nome;
            this.Sobrenome = pessoa.Sobrenome;
            this.Cor = pessoa.Cor;
            this.IdMae = pessoa.IdMae;
            this.IdPai = pessoa.IdPai;
            this.Escolaridade = pessoa.Escolaridade;
            this.RegiaoNascimento = pessoa.RegiaoNascimento;
        }
        public int Nivel { get; set; }
    }
}
