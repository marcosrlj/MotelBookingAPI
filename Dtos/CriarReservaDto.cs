using System;
using System.ComponentModel.DataAnnotations;

namespace MotelBooking.Api.Dtos
{
    /// <summary>
    /// DTO (Data Transfer Object) para criação de novas reservas
    /// </summary>
    public class CriarReservaDto
    {
        /// <summary>
        /// ID do quarto a ser reservado
        /// Marcado como Required para garantir que seja fornecido
        /// </summary>
        [Required]
        public Guid QuartoId { get; set; }

        /// <summary>
        /// Data e hora de início da reserva
        /// Deve ser uma data válida e no formato DateTime
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data e hora de término da reserva
        /// Deve ser uma data válida e no formato DateTime
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DataFim { get; set; }

        /// <summary>
        /// Valida se a reserva atende às regras de negócio:
        /// 1. Data de início deve ser hoje ou posterior
        /// 2. Data de fim deve ser posterior à data de início
        /// </summary>
        /// <returns>True se a reserva é válida, False caso contrário</returns>
        public bool IsValid()
        {
            // Validações de regras de negócio para datas
            return DataInicio.Date >= DateTime.Now.Date && // Data de início deve ser hoje ou futura
                   DataFim.Date > DataInicio.Date;         // Data de fim deve ser após a data de início
        }
    }
}