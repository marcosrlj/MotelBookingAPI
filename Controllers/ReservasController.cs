using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotelBooking.Api.Dtos;
using MotelBooking.Api.Models;
using MotelBooking.Api.Services;
using System.Security.Claims;

namespace MotelBooking.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar as reservas de quartos no motel
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        // Serviço que contém a lógica de negócio para operações com reservas
        private readonly IReservaService _reservaService;

        /// <summary>
        /// Construtor que recebe a injeção de dependência do serviço de reservas
        /// </summary>
        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        /// <summary>
        /// Endpoint para adicionar uma nova reserva
        /// </summary>
        /// <param name="dto">DTO com os dados da reserva a ser criada</param>
        [HttpPost("adicionar")]
        [Authorize] // Requer autenticação
        public async Task<IActionResult> Adicionar([FromBody] CriarReservaDto dto)
        {
            try
            {
                // Obtém o ID do usuário autenticado
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                {
                    return Unauthorized("Usuário não identificado");
                }

                // Valida as datas da reserva
                if (!dto.IsValid())
                {
                    return BadRequest("Datas de reserva inválidas");
                }

                // Verifica disponibilidade do quarto
                var disponibilidade = await _reservaService.VerificarDisponibilidadeAsync(
                    dto.QuartoId,
                    dto.DataInicio,
                    dto.DataFim);

                if (!disponibilidade)
                {
                    return BadRequest("O quarto não está disponível para as datas selecionadas");
                }

                // Cria o objeto de reserva
                var reserva = new Reserva
                {
                    Id = Guid.NewGuid(),
                    UserId = userGuid,
                    QuartoId = dto.QuartoId,
                    DataInicio = dto.DataInicio,
                    DataFim = dto.DataFim,
                    Status = "Pendente"
                };

                // Salva a reserva e retorna 201 Created
                var novaReserva = await _reservaService.AdicionarReservaAsync(reserva);
                return CreatedAtAction(
                    nameof(GetReserva), 
                    new { id = novaReserva.Id }, 
                    novaReserva
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar reserva: {ex.Message}");
            }
        }

        /// <summary>
        /// Endpoint para listar todas as reservas ou apenas as do usuário
        /// </summary>
        [HttpGet("listar")]
        [Authorize]
        public async Task<IActionResult> Listar()
        {
            try
            {
                // Obtém o ID do usuário autenticado
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                {
                    return Unauthorized("Usuário não identificado");
                }

                // Se for admin, retorna todas as reservas
                if (User.IsInRole("Admin"))
                {
                    var todasReservas = await _reservaService.ListarReservasAsync();
                    return Ok(todasReservas);
                }

                // Se não for admin, retorna apenas as reservas do usuário
                var minhasReservas = await _reservaService.ListarReservasPorUsuarioAsync(userGuid);
                return Ok(minhasReservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar reservas: {ex.Message}");
            }
        }

        /// <summary>
        /// Endpoint para listar reservas por mês
        /// </summary>
        [HttpGet("mensal")]
        [Authorize]
        public async Task<IActionResult> ListarReservasMensal([FromQuery] int mes, [FromQuery] int ano)
        {
            try
            {
                // Valida os parâmetros de entrada
                if (mes < 1 || mes > 12 || ano < 1)
                {
                    return BadRequest("Mês ou ano inválido.");
                }

                // Cria as datas em UTC para o início e fim do mês
                var dataInicio = new DateTime(ano, mes, 1, 0, 0, 0, DateTimeKind.Utc);
                var dataFim = dataInicio.AddMonths(1).AddMilliseconds(-1);

                // Obtém as reservas do período
                var reservas = await _reservaService.FiltrarReservasPorDataAsync(dataInicio, dataFim);
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar reservas: {ex.Message}");
            }
        }

        /// <summary>
        /// Endpoint para obter uma reserva específica por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserva(Guid id)
        {
            var reserva = await _reservaService.GetReservaAsync(id);
            if (reserva == null)
                return NotFound("Reserva não encontrada");

            return Ok(reserva);
        }

        /// <summary>
        /// Endpoint alternativo para adicionar reserva (considerar deprecar em favor de /adicionar)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddReserva(Reserva reserva)
        {
            try
            {
                var novaReserva = await _reservaService.AdicionarReservaAsync(reserva);
                return CreatedAtAction(nameof(GetReserva), new { id = novaReserva.Id }, novaReserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}