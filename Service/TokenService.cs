using Microsoft.IdentityModel.Tokens;
using MotelBooking.Api.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Serviço responsável pela geração e validação de tokens JWT
    /// Implementa ITokenService para operações de autenticação
    /// </summary>
    public class TokenService : ITokenService
    {
        /// <summary>
        /// Configuração do aplicativo para acessar as chaves JWT
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor que recebe a configuração via injeção de dependência
        /// </summary>
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gera um token JWT para um usuário específico
        /// </summary>
        public Task<string> GenerateTokenAsync(Usuario usuario)
        {
            // Obtém a chave secreta da configuração
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? 
                throw new InvalidOperationException("JWT Key não configurada"));

            // Define as claims do token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.UserName ?? ""),
                new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                new Claim(ClaimTypes.Role, usuario.Role ?? "User"),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
            };

            // Cria o token JWT com as configurações
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Token válido por 1 hora
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            );

            // Serializa o token para string
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        /// <summary>
        /// Valida um token JWT existente
        /// </summary>
        public Task<bool> ValidateTokenAsync(string token)
        {
            // Verifica se o token está vazio
            if (string.IsNullOrEmpty(token))
                return Task.FromResult(false);

            try
            {
                // Obtém a chave para validação
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? 
                    throw new InvalidOperationException("JWT Key não configurada")));
                
                var tokenHandler = new JwtSecurityTokenHandler();

                // Define os parâmetros de validação
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = key
                };

                // Tenta validar o token
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return Task.FromResult(principal != null);
            }
            catch
            {
                // Retorna falso se houver qualquer erro na validação
                return Task.FromResult(false);
            }
        }
    }
}

