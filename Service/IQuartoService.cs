using MotelBooking.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Interface que define os contratos para o serviço de gerenciamento de quartos
    /// </summary>
    public interface IQuartoService
    {
        /// <summary>
        /// Obtém todos os quartos cadastrados no sistema
        /// </summary>
        /// <returns>Coleção de quartos encontrados</returns>
        Task<IEnumerable<Quarto>> GetQuartosAsync();

        /// <summary>
        /// Busca um quarto específico por seu ID
        /// </summary>
        /// <param name="id">ID do quarto a ser encontrado</param>
        /// <returns>Quarto encontrado ou null se não existir</returns>
        Task<Quarto?> GetQuartoAsync(Guid id);

        /// <summary>
        /// Adiciona um novo quarto ao sistema
        /// </summary>
        /// <param name="quarto">Dados do quarto a ser adicionado</param>
        /// <returns>Quarto criado com seu ID gerado</returns>
        Task<Quarto> AddQuartoAsync(Quarto quarto);

        /// <summary>
        /// Atualiza os dados de um quarto existente
        /// </summary>
        /// <param name="id">ID do quarto a ser atualizado</param>
        /// <param name="quarto">Novos dados do quarto</param>
        /// <returns>Quarto atualizado ou null se não encontrado</returns>
        Task<Quarto?> UpdateQuartoAsync(Guid id, Quarto quarto);

        /// <summary>
        /// Remove um quarto do sistema
        /// </summary>
        /// <param name="id">ID do quarto a ser excluído</param>
        /// <returns>True se excluído com sucesso, False se não encontrado</returns>
        Task<bool> DeleteQuartoAsync(Guid id);

        /// <summary>
        /// Lista todos os quartos ativos no sistema
        /// </summary>
        /// <returns>Lista de quartos disponíveis</returns>
        Task<List<Quarto>> ListarQuartosAsync();
    }
}