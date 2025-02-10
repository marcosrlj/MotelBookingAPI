using Microsoft.EntityFrameworkCore;
using MotelBooking.Api.Data;
using MotelBooking.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Serviço responsável pelo gerenciamento de reservas no sistema
    /// Implementa IReservaService para operações CRUD e regras de negócio
    /// </summary>
    public class ReservaService : IReservaService
    {
        private readonly AppDbContext _context;
        private readonly CacheService _cache;

        /// <summary>
        /// Construtor que recebe as dependências necessárias via DI
        /// </summary>
        public ReservaService(AppDbContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Busca uma reserva específica por ID incluindo relacionamentos
        /// </summary>
        public async Task<Reserva?> GetReservaAsync(Guid id)
        {
            return await _context.Reservas
                .Include(r => r.Quarto)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Verifica disponibilidade de um quarto para um período
        /// </summary>
        public async Task<bool> VerificarDisponibilidadeAsync(Guid quartoId, DateTime dataInicio, DateTime dataFim)
        {
            var conflito = await _context.Reservas.AnyAsync(r =>
                r.QuartoId == quartoId &&
                ((dataInicio >= r.DataInicio && dataInicio < r.DataFim) ||
                 (dataFim > r.DataInicio && dataFim <= r.DataFim)));

            return !conflito;
        }

        /// <summary>
        /// Adiciona uma nova reserva validando a existência do quarto
        /// </summary>
        public async Task<Reserva> AdicionarReservaAsync(Reserva reserva)
        {
            var quarto = await _context.Quartos
                .Include(q => q.Motel)
                .FirstOrDefaultAsync(q => q.Id == reserva.QuartoId);

            if (quarto == null)
                throw new InvalidOperationException("Quarto não encontrado");

            reserva.Quarto = quarto;
            await _context.Reservas.AddAsync(reserva);
            await _context.SaveChangesAsync();

            _cache.Set("ReservasFiltro", null);
            _cache.Set("Faturamento", null);

            return reserva;
        }

        /// <summary>
        /// Atualiza uma reserva existente
        /// </summary>
        public async Task EditarReservaAsync(Guid id, Reserva reserva)
        {
            var reservaExistente = await _context.Reservas.FindAsync(id);
            if (reservaExistente == null)
                throw new InvalidOperationException("Reserva não encontrada");

            reservaExistente.DataInicio = reserva.DataInicio;
            reservaExistente.DataFim = reserva.DataFim;
            reservaExistente.Status = reserva.Status;

            await _context.SaveChangesAsync();
            
            _cache.Set("ReservasFiltro", null);
            _cache.Set("Faturamento", null);
        }

        /// <summary>
        /// Remove uma reserva do sistema
        /// </summary>
        public async Task ExcluirReservaAsync(Guid id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                throw new InvalidOperationException("Reserva não encontrada");

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            _cache.Set("ReservasFiltro", null);
            _cache.Set("Faturamento", null);
        }

        /// <summary>
        /// Lista todas as reservas com seus relacionamentos
        /// </summary>
        public async Task<List<Reserva>> ListarReservasAsync()
        {
            return await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Quarto)
                .ToListAsync();
        }

        /// <summary>
        /// Filtra reservas por usuário específico
        /// </summary>
        public async Task<List<Reserva>> ListarReservasPorUsuarioAsync(Guid usuarioId)
        {
            return await _context.Reservas
                .Where(r => r.UserId == usuarioId)
                .Include(r => r.Usuario)
                .Include(r => r.Quarto)
                .ToListAsync();
        }

        /// <summary>
        /// Filtra reservas por período com suporte a cache
        /// </summary>
        public async Task<List<Reserva>> FiltrarReservasPorDataAsync(DateTime dataInicio, DateTime dataFim)
        {
            if (dataInicio.Kind != DateTimeKind.Utc)
                dataInicio = DateTime.SpecifyKind(dataInicio, DateTimeKind.Utc);
            
            if (dataFim.Kind != DateTimeKind.Utc)
                dataFim = DateTime.SpecifyKind(dataFim, DateTimeKind.Utc);

            string cacheKey = $"Reservas_{dataInicio:yyyyMMdd}_{dataFim:yyyyMMdd}";
            var reservasCached = _cache.Get(cacheKey) as List<Reserva>;
            if (reservasCached != null)
                return reservasCached;

            var reservas = await _context.Reservas
                .Include(r => r.Quarto)
                .Include(r => r.Usuario)
                .Where(r => (r.DataInicio >= dataInicio && r.DataInicio <= dataFim) ||
                           (r.DataFim >= dataInicio && r.DataFim <= dataFim) ||
                           (r.DataInicio <= dataInicio && r.DataFim >= dataFim))
                .OrderBy(r => r.DataInicio)
                .ToListAsync();

            _cache.Set(cacheKey, reservas);
            return reservas;
        }

        /// <summary>
        /// Calcula o faturamento mensal com suporte a cache
        /// </summary>
        public async Task<decimal> ObterFaturamentoMensalAsync(int mes, int ano)
        {
            string cacheKey = $"Faturamento_{mes}_{ano}";
            if (_cache.Get(cacheKey) is decimal faturamentoCached)
                return faturamentoCached;

            decimal faturamento = await _context.Reservas
                .Include(r => r.Quarto)
                .Where(r => r.DataInicio.Month == mes &&
                           r.DataInicio.Year == ano &&
                           r.Status == "Confirmada")
                .SumAsync(r => r.Quarto.Valor * 
                    ((r.DataFim - r.DataInicio).Days + 1));

            _cache.Set(cacheKey, faturamento);
            return faturamento;
        }
    }
}
