using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotelBooking.Api.Models;
using MotelBooking.Api.Services;
using System;
using System.Threading.Tasks;

namespace MotelBooking.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar operações CRUD de quartos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class QuartosController : ControllerBase
    {
        // Serviço que contém a lógica de negócio para operações com quartos
        private readonly IQuartoService _quartoService;

        /// <summary>
        /// Construtor que recebe a injeção de dependência do serviço de quartos
        /// </summary>
        /// <param name="quartoService">Serviço de quartos</param>
        public QuartosController(IQuartoService quartoService)
        {
            _quartoService = quartoService;
        }

        /// <summary>
        /// Endpoint para adicionar um novo quarto ao sistema
        /// </summary>
        /// <param name="quarto">Dados do quarto a ser adicionado</param>
        /// <returns>Retorna o quarto criado com seu ID</returns>
        [HttpPost("adicionar")]
        [Authorize(Roles = "Admin")] // Apenas administradores podem adicionar quartos 
        public async Task<IActionResult> Adicionar([FromBody] Quarto quarto)
        {
            try
            {
                // Validações dos dados de entrada
                if (quarto == null)
                    return BadRequest("Dados do quarto não fornecidos");

                if (quarto.Valor <= 0)
                    return BadRequest("O valor do quarto deve ser maior que zero");

                if (quarto.Numero <= 0)
                    return BadRequest("O número do quarto deve ser maior que zero");

                if (quarto.MotelId == Guid.Empty)
                    return BadRequest("ID do motel não fornecido");

                // Adiciona o quarto e retorna 201 (Created) com o local do recurso
                var novoQuarto = await _quartoService.AddQuartoAsync(quarto);
                return CreatedAtAction(nameof(Obter), new { id = novoQuarto.Id }, novoQuarto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao criar quarto");
            }
        }

        /// <summary>
        /// Endpoint para obter as informações de um quarto específico
        /// </summary>
        /// <param name="id">ID do quarto</param>
        [HttpGet("obter/{id:guid}")]
        public async Task<IActionResult> Obter(Guid id)
        {
            var quarto = await _quartoService.GetQuartoAsync(id);
            if (quarto == null)
                return NotFound();

            return Ok(quarto);
        }

        /// <summary>
        /// Endpoint para atualizar as informações de um quarto existente
        /// </summary>
        /// <param name="id">ID do quarto a ser editado</param>
        /// <param name="quarto">Novos dados do quarto</param>
        [HttpPut("editar/{id}")]
        [Authorize(Roles = "Admin")] // Apenas administradores podem editar quartos
        public async Task<IActionResult> Editar(Guid id, [FromBody] Quarto quarto)
        {
            // Verifica se o quarto existe
            var quartoExistente = await _quartoService.GetQuartoAsync(id);
            if (quartoExistente == null)
            {
                return NotFound("Quarto não encontrado.");
            }

            await _quartoService.UpdateQuartoAsync(id, quarto);
            return NoContent(); // Retorna 204 - No Content
        }

        /// <summary>
        /// Endpoint para remover um quarto do sistema
        /// </summary>
        /// <param name="id">ID do quarto a ser excluído</param>
        [HttpDelete("excluir/{id}")]
        [Authorize(Roles = "Admin")] // Apenas administradores podem excluir quartos
        public async Task<IActionResult> Excluir(Guid id)
        {
            var ok = await _quartoService.DeleteQuartoAsync(id);
            return ok ? NoContent() : NotFound("Quarto não encontrado.");
        }

        /// <summary>
        /// Endpoint para listar todos os quartos cadastrados
        /// </summary>
        /// <returns>Lista de quartos</returns>
        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            var quartos = await _quartoService.ListarQuartosAsync();
            return Ok(quartos);
        }
    }
}
