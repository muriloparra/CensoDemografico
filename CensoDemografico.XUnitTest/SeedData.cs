using CensoDemografico.Domain.Entities;
using CensoDemografico.Domain.Enums;
using CensoDemografico.Infra.Context;
using System.Collections.Generic;

namespace CensoDemografico.XUnitTest
{
    public static class SeedData
    {
        public static void PopulateTestData(CensoDemograficoContext context)
        {
            //Inclui como cadastro principal o meu próprio cadastro

            //Meu pai
            var pai = new Pessoa("Mauro", "Parra");
            context.Pessoas.Add(pai);

            //Minha mãe
            var mae = new Pessoa("Naira", "Parra");
            context.Pessoas.Add(mae);

            //Meu cadastro
            var pessoa = new Pessoa
            {
                Nome = "Murilo",
                Sobrenome = "Parra",
                IdPai = pai.PessoaId,
                IdMae = mae.PessoaId,
                Pai = pai,
                Mae = mae,
                Escolaridade = EEscolaridade.SuperiorCompleto,
                Etnia = EEtnia.Branco,
                Genero = EGenero.Masculino,
                Regiao = ERegiao.Sudeste,
            };
            context.Pessoas.Add(pessoa);

            //Minhas filhas
            var filhos = new List<Pessoa>()
            {
                new Pessoa("Isabella","Parra",pessoa.PessoaId,null),
                new Pessoa("Julia","Parra",pessoa.PessoaId,null),
                new Pessoa("Livia","Parra",pessoa.PessoaId,null),
            };
            context.Pessoas.AddRange(filhos);

            //Salvo no banco em memória
            context.SaveChanges();
        }
    }
}
