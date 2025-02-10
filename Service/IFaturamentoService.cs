using System.Threading.Tasks;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Interface que define os contratos para o serviço de faturamento
    /// Implementada por FaturamentoService
    /// </summary>
    public interface IFaturamentoService
    {
        /// <summary>
        /// Obtém o faturamento total do motel para um mês específico
        /// </summary>
        /// <param name="mes">Mês para cálculo (1-12)</param>
        /// <param name="ano">Ano para cálculo</param>
        /// <returns>Valor total do faturamento no período em decimal</returns>
        Task<decimal> ObterFaturamentoMensalAsync(int mes, int ano);
    }
}
