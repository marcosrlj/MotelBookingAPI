using Microsoft.AspNetCore.Identity;
using System;

namespace MotelBooking.Api.Models
{
    /// <summary>
    /// Modelo que representa um usuário no sistema
    /// Herda de IdentityUser para utilizar o sistema de identidade do ASP.NET Core
    /// </summary>
    public class Usuario : IdentityUser<Guid>
    {
        /// <summary>
        /// Papel/função do usuário no sistema
        /// Valores possíveis: "Admin", "User"
        /// Por padrão, novos usuários recebem o papel "User"
        /// Usado para controle de acesso e autorização
        /// 
        /// Propriedades herdadas do IdentityUser:
        /// - Id (Guid): Identificador único do usuário
        /// - UserName (string): Nome de usuário para login
        /// - Email (string): Endereço de email do usuário
        /// - EmailConfirmed (bool): Indica se o email foi confirmado
        /// - PasswordHash (string): Hash da senha do usuário
        /// - SecurityStamp (string): Usado para invalidar tokens/cookies
        /// - PhoneNumber (string): Número de telefone do usuário
        /// - PhoneNumberConfirmed (bool): Indica se o telefone foi confirmado
        /// - TwoFactorEnabled (bool): Indica se 2FA está habilitado
        /// </summary>
        public string Role { get; set; } = "User";
    }
}