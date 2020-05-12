using CensoDemografico.Api.Controllers.Core;
using CensoDemografico.Domain;
using CensoDemografico.Domain.Command;
using CensoDemografico.Domain.Dtos;
using CensoDemografico.Infra.Transactions;
using CensoDemografico.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Threading.Tasks;

namespace CensoDemografico.Api.Controllers
{
    /// <summary>
    /// Api de cadastro e estatísticas do censo demográfico 2020 versão 1.0
    /// </summary>
    [Route("api/v1/censo")]
    public class CensoController : BaseController
    {
        private readonly IPessoaService _pessoaService;
        public CensoController(IPessoaService pessoaService, IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
            _pessoaService = pessoaService;
        }

        /// <summary>
        /// Cadastrar uma Pessoa no Censo Demográfico
        /// </summary>
        /// 
        /// <remarks>
        /// <example>
        /// Exemplo de requisição:
        /// 
        /// POST /criar
        /// <code>{
        ///    "Nome": "Murilo",
        ///    "Sobrenome": "Parra",
        ///    "NomePai": "Mauro",
        ///    "SobrenomePai": "Parra",
        ///    "NomeMae": "Naira",
        ///    "SobrenomeMae": "Parra",
        ///    "Escolaridade": 4,   //1-Sem Instrução, 2-Ensino Fundamental, 3-Ensino Médio, 4-Superior Completo, 5-Pós-Graduado, 6-Doutorado
        ///    "Etnia": 1,          //1-Branco, 2-Indio, 3-Negro, 4-Pardo
        ///    "Genero": 2,         //1-Feminino, 2-Masculino, 3-NaoInformado
        ///    "Regiao": 4,         //1-CentroOeste, 2-Nordeste, 3-Norte, 4-Sudeste, 5-Sul,
        ///    "Filhos": [
        ///        { 
        ///        	"Nome": "Isabella",
        ///        	"Sobrenome": "Parra"
        ///        },
        ///        {
        ///        	"Nome": "Julia",
        ///        	"Sobrenome": "Parra"
        ///        },
        ///        {
        ///        	"Nome": "Livia",
        ///        	"Sobrenome": "Parra"
        ///        }
        ///    ]
        /// }</code>
        ///     
        /// </example>
        /// </remarks>
        /// <param name="command">Objeto com os dados do indivíduo a ser cadastrado no Censo</param>
        /// <response code="201">Pessoa recém cadastrada</response>
        /// <response code="400">Preencha todos os campos obrigatórios</response>
        [HttpPost]
        [Route("criar")]
        [SwaggerResponse(200, Type = null)]
        public async Task<IActionResult> Criar([FromBody]CriarPessoaCommand command)
        {
            try
            {
                _pessoaService.Criar(command);
                return await Response(null, _pessoaService.Notifications);
            }
            catch (Exception ex)
            {
                return await base.TryErrors(ex);
            }
        }

        /// <summary>
        /// Percentual de Pessoas com Mesmo Nome de uma determinada região
        /// </summary>
        /// <remarks>
        /// <example>
        /// Exemplo de requisição:
        /// 
        /// GET /percentual
        /// <code>
        ///     Murilo
        /// </code>
        /// </example>
        /// </remarks>
        /// <response code="200">Retorna os percentuais por região</response>
        /// <response code="400">Preencha o campo obrigatório</response>
        [HttpGet]
        [Route("percentual")]
        [SwaggerResponse(200,Type = typeof(ResponseResult<PercentualDto>))]
        public async Task<IActionResult> Percentual()
        {
            try
            {
                var result = _pessoaService.Percentual();
                return await Response(result, _pessoaService.Notifications);
            }
            catch (Exception ex)
            {
                return await base.TryErrors(ex);
            }
        }

        /// <summary>
        /// Pesquisa pelo número de indivíduos conforme filtro
        /// </summary>
        /// <remarks>
        /// <example>
        /// Exemplo de requisição:
        /// 
        /// POST /pesquisar
        /// <code>{
        ///    "Nome": "Murilo",
        ///    "Sobrenome": "Parra",
        ///    "Escolaridade": 4,  //1-Sem Instrução, 2-Ensino Fundamental, 3-Ensino Médio, 4-Superior Completo, 5-Pós-Graduado, 6-Doutorado
        ///    "Etnia": 1,         //1-Branco, 2-Indio, 3-Negro, 4-Pardo
        ///    "Genero": 2,        //1-Feminino, 2-Masculino, 3-NaoInformado
        ///    "Regiao": 4         //1-CentroOeste, 2-Nordeste, 3-Norte, 4-Sudeste, 5-Sul
        /// }</code>
        /// </example>
        /// </remarks>
        /// <param name="pesquisa">Objeto com os dados dos filtros a serem utilizados na pesquisa</param>
        /// <response code="200">A quantidade de indivíduos</response>
        /// <response code="400">Preenchimento incorreto dos campos</response>
        [HttpPost]
        [Route("pesquisar")]
        [SwaggerResponse(200, Type = typeof(ResponseResult<int>))]
        public async Task<IActionResult> Pesquisa([FromBody]PesquisaCommand pesquisa)
        {
            try
            {
                var result = _pessoaService.Pesquisa(pesquisa);
                return await Response(result, _pessoaService.Notifications);
            }
            catch (Exception ex)
            {
                return await base.TryErrors(ex);
            }
        }

        /// <summary>
        /// Exibe a árvore genealógica do indivíduo informado
        /// </summary>
        /// <remarks>
        /// <example>
        /// Exemplo de requisição:
        /// 
        /// GET /arvore
        /// <code>
        ///     Murilo
        ///     Parra
        /// </code>
        /// </example>
        /// </remarks>
        /// <param name="nome">Informar o nome do indivíduo</param>
        /// <param name="sobrenome">Informar o sobrenome do indivíduo</param>
        /// <response code="200">Retorna a árvore genealógica desse indivíduo</response>
        /// <response code="404">Não existem dados para esse indivíduo</response>
        [HttpGet]
        [Route("arvore")]
        [SwaggerResponse(200, Type = typeof(ResponseResult<ArvoreRaizDto>))]
        public async Task<IActionResult> Arvore(string nome, string sobrenome)
        {
            try
            {
                var result = _pessoaService.Arvore(nome, sobrenome);
                return await Response(result, _pessoaService.Notifications);
            }
            catch (Exception ex)
            {
                return await base.TryErrors(ex);
            }
        }
    }
}