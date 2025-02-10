using MotelBooking.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Interface que define os contratos para o serviço de gerenciamento de usuários
    /// Responsável por todas as operações relacionadas a usuários no sistema
    /// </summary>
    public interface IUsuarioService
    {
        /// <summary>
        /// Verifica se já existe um usuário com o email fornecido
        /// </summary>
        Task<bool> EmailExisteAsync(string email);

        /// <summary>
        /// Busca um usuário pelo seu ID único
        /// </summary>
        Task<Usuario> ObterUsuarioPorIdAsync(Guid id);

        /// <summary>
        /// Busca um usuário pelo seu endereço de email
        /// </summary>
        Task<Usuario> ObterUsuarioPorEmailAsync(string email);

        /// <summary>
        /// Lista todos os usuários cadastrados no sistema
        /// </summary>
        Task<IEnumerable<Usuario>> ListarUsuariosAsync();

        /// <summary>
        /// Adiciona um novo usuário ao sistema
        /// </summary>
        /// <param name="usuario">Dados do usuário</param>
        /// <param name="senha">Senha em texto plano para ser hasheada</param>
        Task<Usuario> AdicionarUsuarioAsync(Usuario usuario, string senha);

        /// <summary>
        /// Atualiza os dados de um usuário existente
        /// </summary>
        Task<Usuario> EditarUsuarioAsync(Guid id, Usuario usuario);

        /// <summary>
        /// Remove um usuário do sistema
        /// </summary>
        /// <returns>True se excluído com sucesso, False se não encontrado</returns>
        Task<bool> ExcluirUsuarioAsync(Guid id);

        /// <summary>
        /// Autentica um usuário usando email e senha
        /// </summary>
        /// <returns>Token JWT se autenticação bem-sucedida</returns>
        Task<string> AuthenticateAsync(string email, string senha);
    }
}
