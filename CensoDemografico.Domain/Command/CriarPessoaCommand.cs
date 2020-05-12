using CensoDemografico.Domain.Entities;
using CensoDemografico.Domain.Enums;
using System.Collections.Generic;

namespace CensoDemografico.Domain.Command
{
    public class CriarPessoaCommand
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public EEtnia Etnia { get; set; }
        public EGenero Genero { get; set; }
        public string NomePai { get; set; }
        public string SobrenomePai { get; set; }
        public string NomeMae { get; set; }
        public string SobrenomeMae { get; set; }
        public EEscolaridade Escolaridade { get; set; }
        public ERegiao Regiao { get; set; }
        public List<Pessoa> Filhos { get; set; } = new List<Pessoa>();
    }
}
