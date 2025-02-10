using System.ComponentModel.DataAnnotations;

namespace MotelBooking.Api.Models
{
    /// <summary>
    /// Modelo para representar as credenciais de login do usuário
    /// Usado internamente pela aplicação para processar autenticação
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Email do usuário para autenticação
        /// Deve ser um email válido e previamente registrado no sistema
        /// </summary>
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        /// <summary>
        /// Senha do usuário para autenticação
        /// Será comparada com o hash armazenado no banco de dados
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}