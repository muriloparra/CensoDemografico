using System.Collections.Generic;

namespace CensoDemografico.Domain.Dtos
{
    public class ArvoreFilhoDto
    {
        public int? Id { get; set; }
        public string NomeCompleto { get; set; }

        public List<ArvoreFilhoDto> Filhos { get; set; }
    }
}
