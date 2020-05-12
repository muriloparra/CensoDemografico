using CensoDemografico.Domain.Enums;
using CensoDemografico.Domain.ExtensionMethods;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CensoDemografico.Domain.Entities
{
    public class Pessoa
    {
        public Pessoa() { }

        public Pessoa(string nome, string sobrenome)
        {
            Nome = nome;
            Sobrenome = sobrenome;
        }

        public Pessoa(string nome, string sobrenome, int? idPai, int? idMae)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            IdPai = idPai;
            IdMae = idMae;
        }
        [Key]
        public int PessoaId { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public EEtnia Etnia { get; set; }
        public EGenero Genero { get; set; }
        public int? IdPai { get; set; }
        public Pessoa Pai { get; set; }
        public int? IdMae { get; set; }
        public Pessoa Mae { get; set; }
        public EEscolaridade Escolaridade { get; set; }
        public ERegiao Regiao { get; set; }

        public string NomeCompleto() => $"{Nome} {Sobrenome}";
        public string EtniaDescricao() => Etnia.GetEnumDescription();
        public string GeneroDescricao() => Genero.GetEnumDescription();
        public string EscolaridadeDescricao() => Escolaridade.GetEnumDescription();
    }
}
