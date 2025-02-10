using MotelBooking.Api.Models;
using MotelBooking.Api.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Serviço responsável pelo gerenciamento de motéis no sistema
    /// Implementa IMotelService para operações CRUD
    /// </summary>
    public class MotelService : IMotelService
    {
        /// <summary>
        /// Contexto do banco de dados para operações com motéis
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto via injeção de dependência
        /// </summary>
        public MotelService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca um motel específico por seu ID
        /// </summary>
        /// <param name="id">ID do motel a ser encontrado</param>
        /// <returns>Motel encontrado ou null se não existir</returns>
        public async Task<Motel> ObterMotelPorIdAsync(Guid id)
        {
            return await _context.Moteis.FindAsync(id);
        }

        /// <summary>
        /// Adiciona um novo motel ao sistema
        /// </summary>
        /// <param name="motel">Dados do motel a ser adicionado</param>
        /// <returns>Motel criado com seu ID gerado</returns>
        public async Task<Motel> AdicionarMotelAsync(Motel motel)
        {
            await _context.Moteis.AddAsync(motel);
            await _context.SaveChangesAsync();
            return motel;
        }

        /// <summary>
        /// Atualiza os dados de um motel existente
        /// </summary>
        /// <param name="id">ID do motel a ser editado</param>
        /// <param name="motel">Novos dados do motel</param>
        public async Task EditarMotelAsync(Guid id, Motel motel)
        {
            var motelExistente = await _context.Moteis.FindAsync(id);
            if (motelExistente != null)
            {
                motelExistente.Nome = motel.Nome;
                motelExistente.Endereco = motel.Endereco;
                motelExistente.Localizacao = motel.Localizacao;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Remove um motel do sistema
        /// </summary>
        /// <param name="id">ID do motel a ser excluído</param>
        public async Task ExcluirMotelAsync(Guid id)
        {
            var motel = await _context.Moteis.FindAsync(id);
            if (motel != null)
            {
                _context.Moteis.Remove(motel);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Lista todos os motéis cadastrados no sistema
        /// </summary>
        /// <returns>Lista de motéis</returns>
        public async Task<List<Motel>> ListarMoteisAsync()
        {
            return await _context.Moteis.ToListAsync();
        }
    }
}
