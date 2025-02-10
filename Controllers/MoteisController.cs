using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotelBooking.Api.Models;
using MotelBooking.Api.Services;
using System;
using System.Threading.Tasks;

namespace MotelBooking.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar operações CRUD de motéis
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MoteisController : ControllerBase
    {
        // Serviço que contém a lógica de negócio para operações com motéis
        private readonly IMotelService _motelService;

        /// <summary>
        /// Construtor que recebe a injeção de dependência do serviço de motéis
        /// </summary>
        /// <param name="motelService">Serviço de motéis</param>
        public MoteisController(IMotelService motelService)
        {
            _motelService = motelService;
        }

        /// <summary>
        /// Endpoint para adicionar um novo motel ao sistema
        /// </summary>
        /// <param name="motel">Dados do motel a ser adicionado</param>
        /// <returns>Retorna o motel criado com seu ID</returns>
        [HttpPost("adicionar")]
        [Authorize(Roles = "Admin")] // Apenas administradores podem adicionar motéis
        public async Task<IActionResult> Adicionar([FromBody] Motel motel)
        {
            // Validação básica dos dados de entrada
            if (motel == null || string.IsNullOrWhiteSpace(motel.Endereco))
            {
                return BadRequest("O endereço do motel é obrigatório.");
            }

            // Adiciona o motel e retorna 201 (Created) com o local do recurso
            var novoMotel = await _motelService.AdicionarMotelAsync(motel);
            return CreatedAtAction(nameof(Listar), new { id = novoMotel.Id }, novoMotel);
        }

        /// <summary>
        /// Endpoint para atualizar as informações de um motel existente
        /// </summary>
        /// <param name="id">ID do motel a ser editado</param>
        /// <param name="motel">Novos dados do motel</param>
        /// <returns>204 No Content se sucesso</returns>
        [HttpPut("editar/{id}")]
        [Authorize(Roles = "Admin")] // Apenas administradores podem editar motéis
        public async Task<IActionResult> Editar(Guid id, [FromBody] Motel motel)
        {
            // Verifica se o motel existe
            var motelExistente = await _motelService.ObterMotelPorIdAsync(id);
            if (motelExistente == null)
            {
                return NotFound("Motel não encontrado.");
            }

            await _motelService.EditarMotelAsync(id, motel);
            return NoContent(); // Retorna 204 - No Content
        }

        /// <summary>
        /// Endpoint para remover um motel do sistema
        /// </summary>
        /// <param name="id">ID do motel a ser excluído</param>
        /// <returns>204 No Content se sucesso</returns>
        [HttpDelete("excluir/{id}")]
        [Authorize(Roles = "Admin")] // Apenas administradores podem excluir motéis
        public async Task<IActionResult> Excluir(Guid id)
        {
            // Verifica se o motel existe
            var motelExistente = await _motelService.ObterMotelPorIdAsync(id);
            if (motelExistente == null)
            {
                return NotFound("Motel não encontrado.");
            }

            await _motelService.ExcluirMotelAsync(id);
            return NoContent(); // Retorna 204 - No Content
        }

        /// <summary>
        /// Endpoint para listar todos os motéis cadastrados
        /// </summary>
        /// <returns>Lista de motéis</returns>
        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            var moteis = await _motelService.ListarMoteisAsync();
            return Ok(moteis); // Retorna 200 OK com a lista de motéis
        }
    }
}
