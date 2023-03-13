using System;
using System.ComponentModel.DataAnnotations;
using static DesafioArvore.Models.Enums;

namespace DesafioArvore.Domain.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public int? IdPai { get; set; }
        public int? IdMae { get; set; }
        [StringLength(200)]
        public string Nome { get; set; }
        [StringLength(200)]
        public string Sobrenome { get; set; }
        public CorDaPele Cor { get; set; }
        public RegiaoBrasil RegiaoNascimento { get; set; }
        public NivelEscolaridade Escolaridade { get; set; }

    }
}
