using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotelBooking.Api.Services;
using System;
using System.Threading.Tasks;

namespace MotelBooking.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar operações relacionadas ao faturamento do motel
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FaturamentoController : ControllerBase
    {
        // Serviço que contém a lógica de negócio para cálculos de faturamento
        private readonly IFaturamentoService _faturamentoService;

        /// <summary>
        /// Construtor que recebe a injeção de dependência do serviço de faturamento
        /// </summary>
        /// <param name="faturamentoService">Serviço de faturamento</param>
        public FaturamentoController(IFaturamentoService faturamentoService)
        {
            _faturamentoService = faturamentoService;
        }

        /// <summary>
        /// Endpoint para obter o faturamento mensal do motel
        /// </summary>
        /// <param name="mes">Mês para cálculo do faturamento (1-12)</param>
        /// <param name="ano">Ano para cálculo do faturamento</param>
        /// <returns>Retorna o faturamento do mês especificado</returns>
        [HttpGet("mensal")]
        [Authorize(Roles = "Admin")] // Apenas usuários com role Admin podem acessar
        public async Task<IActionResult> FaturamentoMensal([FromQuery] int mes, [FromQuery] int ano)
        {
            try
            {
                // Validação dos parâmetros de entrada
                if (mes < 1 || mes > 12 || ano < 1)
                {
                    return BadRequest("Mês ou ano inválido.");
                }

                // Obtém o faturamento através do serviço
                var faturamento = await _faturamentoService.ObterFaturamentoMensalAsync(mes, ano);

                // Retorna o resultado formatado com informações adicionais
                return Ok(new { 
                    mes = mes,
                    ano = ano,
                    faturamento = faturamento,
                    moeda = "BRL" // Indica que os valores estão em Reais
                });
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna status 500 com a mensagem de erro
                return StatusCode(500, $"Erro ao calcular faturamento: {ex.Message}");
            }
        }
    }
}