using CensoDemografico.Domain.Enums;

namespace CensoDemografico.Domain.Command
{
    public class PesquisaCommand
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public EGenero Genero { get; set; }
        public EEtnia Etnia { get; set; }
        public EEscolaridade Escolaridade { get; set; }
        public ERegiao Regiao { get; set; }
    }
}
