using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MotelBooking.Api.Dtos;
using MotelBooking.Api.Models;
using MotelBooking.Api.Services;
using System;
using System.Threading.Tasks;

namespace MotelBooking.Api.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar operações relacionadas aos usuários do sistema
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        // Serviços injetados via DI para autenticação e gerenciamento de usuários
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Construtor que recebe as dependências necessárias
        /// </summary>
        public UsuariosController(IUsuarioService usuarioService, ITokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Endpoint para autenticação de usuários
        /// </summary>
        /// <param name="loginDto">Credenciais do usuário (email e senha)</param>
        /// <returns>Token JWT para autenticação</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Busca o usuário pelo email
            var usuario = await _usuarioService.ListarUsuariosAsync();
            var usuarioLogado = usuario.FirstOrDefault(u => u.Email == loginDto.Email);

            if (usuarioLogado == null)
            {
                return Unauthorized("Usuário não encontrado");
            }

            // Gera o token JWT
            var token = await _tokenService.GenerateTokenAsync(usuarioLogado);
            return Ok(new { token });
        }

        /// <summary>
        /// Endpoint para registro de novos usuários
        /// </summary>
        /// <param name="dto">Dados do novo usuário</param>
        [HttpPost("adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] RegisterUsuarioDto dto)
        {
            // Verifica se o email já está em uso
            if (await _usuarioService.EmailExisteAsync(dto.Email))
            {
                return BadRequest("Este e-mail já está em uso.");
            }

            // Cria o novo usuário
            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                UserName = dto.UserName,
                Role = dto.Role
            };

            // Tenta adicionar o usuário
            var novoUsuario = await _usuarioService.AdicionarUsuarioAsync(usuario, dto.Senha);
            if (novoUsuario == null)
            {
                return BadRequest("Erro ao criar usuário.");
            }

            return CreatedAtAction(nameof(Listar), new { id = novoUsuario.Id }, novoUsuario);
        }

        /// <summary>
        /// Endpoint para edição de usuários (apenas Admin)
        /// </summary>
        [HttpPut("editar/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editar(Guid id, [FromBody] Usuario usuario)
        {
            var usuarioExistente = await _usuarioService.ObterUsuarioPorIdAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            await _usuarioService.EditarUsuarioAsync(id, usuario);
            return NoContent();
        }

        /// <summary>
        /// Endpoint para exclusão de usuários (apenas Admin)
        /// </summary>
        [HttpDelete("excluir/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var usuarioExistente = await _usuarioService.ObterUsuarioPorIdAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            await _usuarioService.ExcluirUsuarioAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Endpoint para listar todos os usuários (apenas Admin)
        /// </summary>
        [HttpGet("listar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Listar()
        {
            var usuarios = await _usuarioService.ListarUsuariosAsync();
            return Ok(usuarios);
        }

        /// <summary>
        /// Endpoint para usuário visualizar seu próprio perfil
        /// </summary>
        [HttpGet("perfil")]
        [Authorize]
        public async Task<IActionResult> ObterPerfil()
        {
            var usuarioId = Guid.Parse(User.Identity.Name);
            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(usuarioId);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(usuario);
        }
    }
}
