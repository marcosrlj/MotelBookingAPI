using MotelBooking.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Interface que define os contratos para o serviço de gerenciamento de reservas
    /// </summary>
    public interface IReservaService
    {
        /// <summary>
        /// Verifica se um quarto está disponível para reserva em um período específico
        /// </summary>
        /// <returns>True se disponível, False se já reservado</returns>
        Task<bool> VerificarDisponibilidadeAsync(Guid quartoId, DateTime dataInicio, DateTime dataFim);

        /// <summary>
        /// Adiciona uma nova reserva ao sistema
        /// </summary>
        /// <returns>Reserva criada com seu ID gerado</returns>
        Task<Reserva> AdicionarReservaAsync(Reserva reserva);

        /// <summary>
        /// Busca uma reserva específica por seu ID
        /// </summary>
        /// <returns>Reserva encontrada ou null se não existir</returns>
        Task<Reserva> GetReservaAsync(Guid id);

        /// <summary>
        /// Atualiza os dados de uma reserva existente
        /// </summary>
        Task EditarReservaAsync(Guid id, Reserva reserva);

        /// <summary>
        /// Remove uma reserva do sistema
        /// </summary>
        Task ExcluirReservaAsync(Guid id);

        /// <summary>
        /// Lista todas as reservas cadastradas no sistema
        /// </summary>
        /// <returns>Lista de todas as reservas</returns>
        Task<List<Reserva>> ListarReservasAsync();

        /// <summary>
        /// Lista todas as reservas de um usuário específico
        /// </summary>
        /// <returns>Lista de reservas do usuário</returns>
        Task<List<Reserva>> ListarReservasPorUsuarioAsync(Guid usuarioId);

        /// <summary>
        /// Filtra reservas por período específico
        /// </summary>
        /// <returns>Lista de reservas no período</returns>
        Task<List<Reserva>> FiltrarReservasPorDataAsync(DateTime dataInicio, DateTime dataFim);

        /// <summary>
        /// Calcula o faturamento total de um mês específico
        /// </summary>
        /// <returns>Valor total do faturamento no período</returns>
        Task<decimal> ObterFaturamentoMensalAsync(int mes, int ano);
    }
}
