using CensoDemografico.Api;
using CensoDemografico.Api.Controllers.Core;
using CensoDemografico.Domain.Command;
using CensoDemografico.Domain.Dtos;
using CensoDemografico.Domain.Enums;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CensoDemografico.XUnitTest.Controllers
{
    public class CensoControllerUnitTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public CensoControllerUnitTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Criar_CadastroDoCenso()
        {
            //Consigo inserir minha irmã que não possui filhos
            var pessoa = new CriarPessoaCommand
            {
                Nome = "Milena",
                Sobrenome = "Parra",
                NomePai = "Mauro",
                SobrenomePai = "Parra",
                NomeMae = "Naira",
                SobrenomeMae = "Parra",
                Escolaridade = EEscolaridade.SuperiorCompleto,
                Etnia = EEtnia.Branco,
                Genero = EGenero.Feminino,
                Regiao = ERegiao.Sudeste
            };

            var json = JsonConvert.SerializeObject(pessoa);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/v1/censo/criar", data);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Percentual_100PorCentoParaSudoeste()
        {
            var response = await _client.GetAsync("/api/v1/censo/percentual");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseResult<PercentualDto>>(stringResponse);

            decimal expected = 100.0m;
            decimal actual = result.Data.regiaoSudeste;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Pesquisa_DeveRetornarOValorCorretoParaDuasPessoasComGrauSuperiorCompleto()
        {
            var pesquisa = new PesquisaCommand
            {
                Escolaridade = EEscolaridade.SuperiorCompleto
            };

            var json = JsonConvert.SerializeObject(pesquisa);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/v1/censo/pesquisar", data);
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseResult<int>>(stringResponse);
            int expected = 1;
            int actual = result.Data;
            Assert.Equal(expected, actual);
        }
    }
}
