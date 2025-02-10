using System;
using System.ComponentModel.DataAnnotations;

namespace MotelBooking.Api.Models
{
    /// <summary>
    /// Modelo que representa uma reserva de quarto no sistema
    /// Gerencia o relacionamento entre usuários e quartos, incluindo período e status
    /// </summary>
    public class Reserva
    {
        /// <summary>
        /// Identificador único da reserva
        /// Gerado automaticamente pelo banco de dados
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID do usuário que fez a reserva
        /// Chave estrangeira para a tabela de usuários
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// ID do quarto reservado
        /// Chave estrangeira para a tabela de quartos
        /// </summary>
        public Guid QuartoId { get; set; }

        /// <summary>
        /// Data e hora de início da reserva
        /// Armazenada em UTC para consistência entre fusos horários
        /// </summary>
        [Required]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data e hora de fim da reserva
        /// Armazenada em UTC para consistência entre fusos horários
        /// </summary>
        [Required]
        public DateTime DataFim { get; set; }

        /// <summary>
        /// Status da reserva (Pendente, Confirmada)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        /// <summary>
        /// Relação com o usuário
        /// </summary>
        public Usuario Usuario { get; set; }

        /// <summary>
        /// Relação com o quarto
        /// </summary>
        public Quarto Quarto { get; set; }
    }
}