using CensoDemografico.Domain.Dtos;
using CensoDemografico.Domain.Entities;
using CensoDemografico.Infra.Context;
using CensoDemografico.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CensoDemografico.Infra.Repositories
{
    public class PessoaRepository : BaseRepository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(CensoDemograficoContext censoDemograficoContext) : base(censoDemograficoContext)
        {
            base._context = censoDemograficoContext;
        }

        public void AdicionaOuAtualiza(Pessoa pessoa, int? idPai, int? idMae)
        {
            var result = BuscaPorNomeSobrenome(pessoa.Nome, pessoa.Sobrenome);
            if (result == null)
            {
                base.Add(new Pessoa(pessoa.Nome, pessoa.Sobrenome, idPai ?? null, idMae ?? null));
            }
            else
            {
                result.IdPai = idPai ?? null;
                result.IdMae = idMae ?? null;
                base.Update(result);
            }
        }

        public Pessoa BuscaPorNomeSobrenome(string nome, string sobrenome)
        {
            var query = _context.Set<Pessoa>().AsQueryable().AsNoTracking()
                .Where(x => x.Nome.Equals(nome) && x.Sobrenome.Equals(sobrenome))
                .FirstOrDefault();
            if (query == null) return null;

            return query;
        }

        public List<ArvoreFilhoDto> BuscaFilhos(int id)
        {
            var query = _context.Pessoas.AsQueryable().AsNoTracking()
                .Where(p => p.IdPai == id || p.IdMae == id);
            if (query == null) return null;

            return query.Select(p => new ArvoreFilhoDto { Id = p.PessoaId, NomeCompleto = p.NomeCompleto() }).ToList();
        }
    }
}
