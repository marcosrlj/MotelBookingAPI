using MotelBooking.Api.Models; 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Interface que define os contratos para o serviço de gerenciamento de motéis
    /// </summary>
    public interface IMotelService
    {
        /// <summary>
        /// Adiciona um novo motel ao sistema
        /// </summary>
        /// <param name="motel">Dados do motel a ser adicionado</param>
        /// <returns>Motel criado com seu ID gerado</returns>
        Task<Motel> AdicionarMotelAsync(Motel motel);

        /// <summary>
        /// Busca um motel específico por seu ID
        /// </summary>
        /// <param name="id">ID do motel a ser encontrado</param>
        /// <returns>Motel encontrado ou null se não existir</returns>
        Task<Motel> ObterMotelPorIdAsync(Guid id);

        /// <summary>
        /// Atualiza os dados de um motel existente
        /// </summary>
        /// <param name="id">ID do motel a ser editado</param>
        /// <param name="motel">Novos dados do motel</param>
        Task EditarMotelAsync(Guid id, Motel motel);

        /// <summary>
        /// Remove um motel do sistema
        /// </summary>
        /// <param name="id">ID do motel a ser excluído</param>
        Task ExcluirMotelAsync(Guid id);

        /// <summary>
        /// Lista todos os motéis cadastrados no sistema
        /// </summary>
        /// <returns>Lista de motéis</returns>
        Task<List<Motel>> ListarMoteisAsync();
    }
}
