using System.ComponentModel.DataAnnotations;

namespace MotelBooking.Api.Dtos
{
    /// <summary>
    /// DTO (Data Transfer Object) para registro de novos usuários no sistema
    /// Contém todas as informações necessárias para criar uma nova conta
    /// </summary>
    public class RegisterUsuarioDto
    {
        /// <summary>
        /// Email do usuário - usado como identificador único
        /// Deve ser um endereço de email válido
        /// </summary>
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public required string Email { get; set; }

        /// <summary>
        /// Nome de usuário para exibição no sistema
        /// </summary>
        [Required(ErrorMessage = "O nome de usuário é obrigatório")]
        [MinLength(3, ErrorMessage = "O nome de usuário deve ter pelo menos 3 caracteres")]
        public required string UserName { get; set; }

        /// <summary>
        /// Papel/função do usuário no sistema (ex: "Admin", "User")
        /// Define as permissões de acesso
        /// </summary>
        [Required(ErrorMessage = "O papel do usuário é obrigatório")]
        public required string Role { get; set; }

        /// <summary>
        /// Senha do usuário
        /// Será hashada antes de ser armazenada no banco
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        [DataType(DataType.Password)]
        public required string Senha { get; set; }
    }
}