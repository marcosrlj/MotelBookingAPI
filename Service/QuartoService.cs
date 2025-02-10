using Microsoft.EntityFrameworkCore;
using MotelBooking.Api.Data;
using MotelBooking.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Serviço responsável pelo gerenciamento de quartos no sistema
    /// Implementa IQuartoService para operações CRUD
    /// </summary>
    public class QuartoService : IQuartoService
    {
        /// <summary>
        /// Contexto do banco de dados para operações com quartos
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto via injeção de dependência
        /// </summary>
        public QuartoService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os quartos cadastrados incluindo dados do motel
        /// </summary>
        public async Task<IEnumerable<Quarto>> GetQuartosAsync()
        {
            return await _context.Quartos
                .Include(q => q.Motel)
                .ToListAsync();
        }

        /// <summary>
        /// Busca um quarto específico por seu ID
        /// </summary>
        public async Task<Quarto?> GetQuartoAsync(Guid id)
        {
            return await _context.Quartos
                .Include(q => q.Motel)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        /// <summary>
        /// Adiciona um novo quarto ao sistema
        /// Verifica se o motel existe antes de criar o quarto
        /// </summary>
        public async Task<Quarto> AddQuartoAsync(Quarto quarto)
        {
            var motel = await _context.Moteis.FindAsync(quarto.MotelId);
            if (motel == null)
                throw new InvalidOperationException("Motel não encontrado");

            var novoQuarto = new Quarto
            {
                Id = Guid.NewGuid(),
                Numero = quarto.Numero,
                Valor = quarto.Valor,
                MotelId = motel.Id,
                Motel = motel
            };

            await _context.Quartos.AddAsync(novoQuarto);
            await _context.SaveChangesAsync();

            return novoQuarto;
        }

        /// <summary>
        /// Atualiza os dados de um quarto existente
        /// Verifica existência do quarto e do motel
        /// </summary>
        public async Task<Quarto?> UpdateQuartoAsync(Guid id, Quarto quarto)
        {
            var existingQuarto = await _context.Quartos
                .Include(q => q.Motel)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (existingQuarto == null)
                return null;

            var motel = await _context.Moteis.FindAsync(quarto.MotelId);
            if (motel == null)
                throw new InvalidOperationException("Motel não encontrado");

            existingQuarto.Numero = quarto.Numero;
            existingQuarto.Valor = quarto.Valor;
            existingQuarto.MotelId = motel.Id;
            existingQuarto.Motel = motel;

            await _context.SaveChangesAsync();

            return existingQuarto;
        }

        /// <summary>
        /// Remove um quarto do sistema
        /// Retorna true se removido com sucesso
        /// </summary>
        public async Task<bool> DeleteQuartoAsync(Guid id)
        {
            var quarto = await _context.Quartos.FindAsync(id);
            if (quarto == null)
                return false;

            _context.Quartos.Remove(quarto);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Lista todos os quartos ativos incluindo dados do motel
        /// </summary>
        public async Task<List<Quarto>> ListarQuartosAsync()
        {
            return await _context.Quartos
                .Include(q => q.Motel)
                .ToListAsync();
        }
    }
}
