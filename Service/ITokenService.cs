using System.Threading.Tasks;
using MotelBooking.Api.Models;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Interface para gerenciamento de tokens JWT (JSON Web Tokens)
    /// Responsável pela geração e validação de tokens de autenticação
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Gera um token JWT para um usuário específico
        /// </summary>
        /// <param name="usuario">Usuário para qual o token será gerado</param>
        /// <returns>Token JWT em formato string</returns>
        /// <remarks>
        /// O token conterá claims como:
        /// - ID do usuário
        /// - Email
        /// - Role (papel/função)
        /// - Data de expiração
        /// </remarks>
        Task<string> GenerateTokenAsync(Usuario usuario);

        /// <summary>
        /// Valida um token JWT existente
        /// </summary>
        /// <param name="token">Token JWT a ser validado</param>
        /// <returns>True se o token for válido, False caso contrário</returns>
        /// <remarks>
        /// Verifica:
        /// - Assinatura do token
        /// - Data de expiração
        /// - Formato correto
        /// </remarks>
        Task<bool> ValidateTokenAsync(string token);
    }
}
