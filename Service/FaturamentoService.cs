using Microsoft.EntityFrameworkCore;
using MotelBooking.Api.Data;
using MotelBooking.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Serviço responsável por calcular o faturamento do motel
    /// </summary>
    public class FaturamentoService : IFaturamentoService
    {
        // Contexto do banco de dados para acesso às reservas
        private readonly AppDbContext _context;
        // Serviço de cache para otimizar consultas frequentes
        private readonly CacheService _cache;

        /// <summary>
        /// Construtor que recebe as dependências necessárias
        /// </summary>
        public FaturamentoService(AppDbContext context, CacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Calcula o faturamento total do motel para um mês específico
        /// </summary>
        /// <param name="mes">Mês para cálculo (1-12)</param>
        /// <param name="ano">Ano para cálculo</param>
        /// <returns>Valor total do faturamento no período</returns>
        public async Task<decimal> ObterFaturamentoMensalAsync(int mes, int ano)
        {
            // Tenta obter o resultado do cache primeiro
            string cacheKey = $"Faturamento_{mes}_{ano}";
            if (_cache.Get(cacheKey) is decimal faturamentoCached)
            {
                return faturamentoCached;
            }

            // Cria as datas em UTC para o início e fim do mês
            var dataInicio = new DateTime(ano, mes, 1, 0, 0, 0, DateTimeKind.Utc);
            var dataFim = dataInicio.AddMonths(1).AddSeconds(-1);

            // Busca todas as reservas do mês que atendem aos critérios
            var reservasDoMes = await _context.Reservas
                .Include(r => r.Quarto)
                .Where(r => 
                    (r.Status == "Confirmada" || r.Status == "Pendente") && // Incluir reservas pendentes também
                    ((r.DataInicio >= dataInicio && r.DataInicio <= dataFim) || // Começa no mês
                     (r.DataFim >= dataInicio && r.DataFim <= dataFim) ||      // Termina no mês
                     (r.DataInicio <= dataInicio && r.DataFim >= dataFim)))    // Engloba o mês
                .ToListAsync();

            decimal faturamentoTotal = 0;

            // Calcula o valor total de cada reserva
            foreach (var reserva in reservasDoMes)
            {
                // Calcula os dias da reserva
                int diasReserva = (reserva.DataFim - reserva.DataInicio).Days + 1;
                
                // Multiplica o valor diário do quarto pelo número de dias
                decimal valorReserva = reserva.Quarto.Valor * diasReserva;
                
                // Soma ao faturamento total
                faturamentoTotal += valorReserva;
            }

            // Armazena o resultado em cache para futuras consultas
            _cache.Set(cacheKey, faturamentoTotal);
            return faturamentoTotal;
        }
    }
}
