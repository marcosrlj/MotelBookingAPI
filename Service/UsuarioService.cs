using Microsoft.AspNetCore.Identity;
using MotelBooking.Api.Data;
using MotelBooking.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Serviço responsável pelo gerenciamento de usuários
    /// Implementa IUsuarioService usando Identity para autenticação
    /// </summary>
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Construtor com injeção de dependências necessárias
        /// Inclui validações para garantir que as dependências não são nulas
        /// </summary>
        public UsuarioService(
            UserManager<Usuario> userManager,
            AppDbContext context,
            ITokenService tokenService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        /// <summary>
        /// Verifica se um email já está em uso
        /// </summary>
        Task<bool> IUsuarioService.EmailExisteAsync(string email)
        {
            return Task.FromResult(_userManager.FindByEmailAsync(email) != null);
        }

        /// <summary>
        /// Busca um usuário pelo seu ID
        /// </summary>
        Task<Usuario> IUsuarioService.ObterUsuarioPorIdAsync(Guid id)
        {
            return _userManager.FindByIdAsync(id.ToString());
        }

        /// <summary>
        /// Busca um usuário pelo seu email
        /// </summary>
        Task<Usuario> IUsuarioService.ObterUsuarioPorEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Lista todos os usuários cadastrados
        /// </summary>
        Task<IEnumerable<Usuario>> IUsuarioService.ListarUsuariosAsync()
        {
            return Task.FromResult<IEnumerable<Usuario>>(_userManager.Users);
        }

        /// <summary>
        /// Adiciona um novo usuário ao sistema
        /// Inclui criação de senha hasheada e atribuição de papel
        /// </summary>
        Task<Usuario> IUsuarioService.AdicionarUsuarioAsync(Usuario usuario, string senha)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            if (string.IsNullOrEmpty(senha)) throw new ArgumentNullException(nameof(senha));

            var result = _userManager.CreateAsync(usuario, senha).Result;
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Erro ao criar usuário: {string.Join(", ", result.Errors)}");
            }
            
            _userManager.AddToRoleAsync(usuario, usuario.Role ?? "User").Wait();
            return Task.FromResult(usuario);
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente
        /// </summary>
        Task<Usuario> IUsuarioService.EditarUsuarioAsync(Guid id, Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            var existingUser = _userManager.FindByIdAsync(id.ToString()).Result;
            if (existingUser == null)
                return Task.FromResult<Usuario>(null);

            existingUser.Email = usuario.Email;
            existingUser.UserName = usuario.UserName;
            existingUser.Role = usuario.Role;

            var result = _userManager.UpdateAsync(existingUser).Result;
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Erro ao atualizar usuário: {string.Join(", ", result.Errors)}");
            }

            return Task.FromResult(existingUser);
        }

        /// <summary>
        /// Remove um usuário do sistema
        /// </summary>
        Task<bool> IUsuarioService.ExcluirUsuarioAsync(Guid id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            if (user == null)
                return Task.FromResult(false);

            var result = _userManager.DeleteAsync(user).Result;
            return Task.FromResult(result.Succeeded);
        }

        /// <summary>
        /// Autentica um usuário e gera um token JWT
        /// </summary>
        Task<string> IUsuarioService.AuthenticateAsync(string email, string senha)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user == null)
                return Task.FromResult<string>(null);

            var isValid = _userManager.CheckPasswordAsync(user, senha).Result;
            if (!isValid)
                return Task.FromResult<string>(null);

            return _tokenService.GenerateTokenAsync(user);
        }
    }
}
