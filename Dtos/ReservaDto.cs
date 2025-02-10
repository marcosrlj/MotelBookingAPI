using System;

namespace MotelBooking.Api.Dtos
{
    /// <summary>
    /// DTO (Data Transfer Object) para transferência de dados de reservas
    /// Usado para retornar informações simplificadas de reservas nas APIs
    /// </summary>
    public class ReservaDto
    {
        /// <summary>
        /// Identificador único do quarto reservado
        /// </summary>
        public Guid QuartoId { get; set; }

        /// <summary>
        /// Data e hora de início da reserva
        /// Armazenada em UTC para consistência entre fusos horários
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data e hora de término da reserva
        /// Armazenada em UTC para consistência entre fusos horários
        /// </summary>
        public DateTime DataFim { get; set; }

        /// <summary>
        /// Status atual da reserva
        /// Valores possíveis: "Pendente", "Confirmada", "Cancelada"
        /// Campo obrigatório usando o modificador required do C# 11
        /// </summary>
        public required string Status { get; set; }
    }
}
