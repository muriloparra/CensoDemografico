using CensoDemografico.Domain.Command;
using CensoDemografico.Domain.Dtos;
using CensoDemografico.Domain.Entities;
using CensoDemografico.Domain.Enums;
using CensoDemografico.Infra.Repositories.Interfaces;
using CensoDemografico.Services.Interfaces;
using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace CensoDemografico.Services
{
    public class PessoaService : Notifiable, IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }

        #region Gerar Cadastro do Censo Demográfico

        public void Criar(CriarPessoaCommand command)
        {
            using (var trans = new TransactionScope())
            {
                var pessoa = _pessoaRepository.BuscaPorNomeSobrenome(command.Nome, command.Sobrenome)
                    ?? new Pessoa(command.Nome, command.Sobrenome);

                if (string.IsNullOrWhiteSpace(command.Nome) ||
                    string.IsNullOrWhiteSpace(command.Sobrenome))
                {
                    this.AddNotification("Nome e Sobrenome", "Nome e Sobrenome são obrigatórios");
                }
                if (this.Invalid) return;

                pessoa.Genero = command.Genero;
                pessoa.Etnia = command.Etnia;
                pessoa.Escolaridade = command.Escolaridade;
                pessoa.Regiao = command.Regiao;

                if (command.Regiao == 0)
                    this.AddNotification("Regiao", "A região é obrigatória para o censo");
                if (this.Invalid) return;

                pessoa.Pai = ExistePai(command);
                pessoa.Mae = ExisteMae(command);

                //Adiciona a pessoa
                _pessoaRepository.AdicionaOuAtualiza(pessoa, pessoa.Pai.PessoaId, pessoa.Mae.PessoaId);

                //Adiciona os filhos
                AdicionaFilhos(command.Filhos, command.Genero, pessoa);

                trans.Complete();
            }
        }

        private void AdicionaFilhos(List<Pessoa> filhos, EGenero genero, Pessoa pessoa)
        {
            foreach (var filho in filhos)
            {
                if (genero == EGenero.Masculino || (genero == EGenero.NaoInformado))
                {
                    _pessoaRepository.AdicionaOuAtualiza(
                        new Pessoa(filho.Nome, filho.Sobrenome, pessoa.PessoaId, null),
                        pessoa.PessoaId, null);
                }
                else if (genero == EGenero.Feminino)
                {
                    _pessoaRepository.AdicionaOuAtualiza(
                        new Pessoa(filho.Nome, filho.Sobrenome, null, pessoa.PessoaId),
                        null, pessoa.PessoaId);
                }
            }
        }

        private Pessoa ExistePai(CriarPessoaCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.NomePai))
                return null;

            var pai = _pessoaRepository.BuscaPorNomeSobrenome(command.NomePai, command.SobrenomePai);
            if (pai == null)
            {
                _pessoaRepository.Add(new Pessoa(command.NomePai, command.SobrenomePai));
                pai = _pessoaRepository.BuscaPorNomeSobrenome(command.NomePai, command.SobrenomePai);
            }
            return pai;
        }

        private Pessoa ExisteMae(CriarPessoaCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.NomeMae))
                return null;

            var mae = _pessoaRepository.BuscaPorNomeSobrenome(command.NomeMae, command.SobrenomeMae);
            if (mae == null)
            {
                _pessoaRepository.Add(new Pessoa(command.NomeMae, command.SobrenomeMae));
                mae = _pessoaRepository.BuscaPorNomeSobrenome(command.NomeMae, command.SobrenomeMae);
            }
            return mae;
        }

        #endregion

        #region Percentual Nomes Por Regiao

        public PercentualDto Percentual() =>
            new PercentualDto
            {
                regiaoCentroOeste = CalculaPercentualRegiao(ERegiao.CentroOeste),
                regiaoNordeste = CalculaPercentualRegiao(ERegiao.Nordeste),
                regiaoNorte = CalculaPercentualRegiao(ERegiao.Norte),
                regiaoSudeste = CalculaPercentualRegiao(ERegiao.Sudeste),
                regiaoSul = CalculaPercentualRegiao(ERegiao.Sul),
            };

        private decimal CalculaPercentualRegiao(ERegiao regiao)
        {
            var query = _pessoaRepository.AllAsNoTracking;

            int totalPorRegiao = query.Where(p => p.Regiao != 0).Count();

            if (totalPorRegiao == 0) return Decimal.Zero;

            query = query.Where(p => p.Regiao == regiao);

            return Decimal.Round(query.Count() * 100 / totalPorRegiao, 2);
        }

        #endregion

        #region Pesquisa de número de indivíduos

        public int Pesquisa(PesquisaCommand pesquisa)
        {
            var query = _pessoaRepository.AllAsNoTracking;

            if (!string.IsNullOrWhiteSpace(pesquisa.Nome))
                query = query.Where(p => p.Nome.Equals(pesquisa.Nome));

            if (!string.IsNullOrWhiteSpace(pesquisa.Sobrenome))
                query = query.Where(p => p.Sobrenome.Equals(pesquisa.Sobrenome));

            if (pesquisa.Genero != 0)
                query = query.Where(p => p.Genero == pesquisa.Genero);

            if (pesquisa.Etnia != 0)
                query = query.Where(p => p.Etnia == pesquisa.Etnia);

            if (pesquisa.Regiao != 0)
                query = query.Where(p => p.Regiao == pesquisa.Regiao);

            if (pesquisa.Escolaridade != 0)
                query = query.Where(p => p.Escolaridade == pesquisa.Escolaridade);

            return query.Count();
        }

        #endregion

        #region Árvore Genealógica

        public ArvoreRaizDto Arvore(string nome, string sobrenome)
        {
            var pessoaRaiz = _pessoaRepository.BuscaPorNomeSobrenome(nome, sobrenome);
            if (pessoaRaiz == null)
                this.AddNotification("pessoaRaiz", "Árvore não existente para essa pessoa");
            if (this.Invalid) return null;

            pessoaRaiz.Pai = pessoaRaiz.IdPai == null ? null : BuscaAscendente(pessoaRaiz.IdPai);
            pessoaRaiz.Mae = pessoaRaiz.IdMae == null ? null : BuscaAscendente(pessoaRaiz.IdMae);

            var pessoaRaizDto = new ArvoreFilhoDto()
            {
                Id = pessoaRaiz.PessoaId,
                NomeCompleto = pessoaRaiz.NomeCompleto(),
                Filhos = null
            };

            BuscaFilhos(pessoaRaizDto);

            return new ArvoreRaizDto()
            {
                NomeCompleto = pessoaRaiz.NomeCompleto(),
                NomeCompletoPai = pessoaRaiz.Pai?.NomeCompleto(),
                NomeCompletoMae = pessoaRaiz.Mae?.NomeCompleto(),
                Filhos = pessoaRaizDto.Filhos
            };
        }

        private void BuscaFilhos(ArvoreFilhoDto pessoa)
        {
            var busca = _pessoaRepository.BuscaFilhos(pessoa.Id.Value).ToList();

            if (busca.Count > 0)
            {
                foreach (ArvoreFilhoDto filho in busca)
                {
                    BuscaFilhos(filho);
                    if (pessoa.Filhos == null) pessoa.Filhos = new List<ArvoreFilhoDto>();
                    pessoa.Filhos.Add(filho);
                }
            }
        }

        private Pessoa BuscaAscendente(int? id)
        {
            if (id == null) return null;
            return _pessoaRepository.Find(id.Value);
        }

        #endregion
    }
}
