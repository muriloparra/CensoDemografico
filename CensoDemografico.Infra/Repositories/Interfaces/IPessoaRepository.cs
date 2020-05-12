using CensoDemografico.Domain.Dtos;
using CensoDemografico.Domain.Entities;
using System.Collections.Generic;

namespace CensoDemografico.Infra.Repositories.Interfaces
{
    public interface IPessoaRepository : IBaseRepository<Pessoa>
    {
        void AdicionaOuAtualiza(Pessoa pessoa, int? idPai, int? idMae);
        Pessoa BuscaPorNomeSobrenome(string nome, string sobrenome);
        List<ArvoreFilhoDto> BuscaFilhos(int id);
    }
}
