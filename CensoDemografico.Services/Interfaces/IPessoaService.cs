using CensoDemografico.Domain.Command;
using CensoDemografico.Domain.Contracts;
using CensoDemografico.Domain.Dtos;

namespace CensoDemografico.Services.Interfaces
{
    public interface IPessoaService : INotifiable
    {
        void Criar(CriarPessoaCommand pessoa);
        PercentualDto Percentual();
        int Pesquisa(PesquisaCommand pesquisa);
        ArvoreRaizDto Arvore(string nome, string sobrenome);
    }
}