using System.ComponentModel.DataAnnotations;

namespace MotelBooking.Api.Dtos
{
    /// <summary>
    /// DTO (Data Transfer Object) para autenticação de usuários
    /// Contém as credenciais necessárias para login no sistema
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Email do usuário
        /// Usado como identificador único no sistema
        /// Obrigatório para realizar login
        /// </summary>
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        public required string Email { get; set; }

        /// <summary>
        /// Senha do usuário
        /// Obrigatória para realizar login
        /// Será validada contra o hash armazenado no banco
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        public required string Senha { get; set; }
    }
}
