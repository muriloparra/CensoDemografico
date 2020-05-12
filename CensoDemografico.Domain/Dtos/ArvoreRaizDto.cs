using System.Collections.Generic;

namespace CensoDemografico.Domain.Dtos
{
    public class ArvoreRaizDto
    {
        public string NomeCompleto { get; set; }
        public string NomeCompletoPai { get; set; }
        public string NomeCompletoMae { get; set; }
        public List<ArvoreFilhoDto> Filhos { get; set; }
    }
}
