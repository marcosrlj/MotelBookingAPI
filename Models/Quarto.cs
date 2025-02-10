using System;
using System.ComponentModel.DataAnnotations;

namespace MotelBooking.Api.Models
{
    /// <summary>
    /// Modelo que representa um quarto de motel no sistema
    /// Contém informações sobre identificação, preço e relacionamento com o motel
    /// </summary>
    public class Quarto
    {
        /// <summary>
        /// Construtor que inicializa um novo quarto com ID único
        /// </summary>
        public Quarto()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Identificador único do quarto
        /// Gerado automaticamente no construtor
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Número do quarto no motel
        /// Deve ser um número positivo e único dentro do motel
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "O número do quarto deve ser maior que zero")]
        public int Numero { get; set; }

        /// <summary>
        /// Valor da diária do quarto
        /// Armazenado como decimal para precisão monetária
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do quarto deve ser maior que zero")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Chave estrangeira para o motel ao qual o quarto pertence
        /// </summary>
        public Guid MotelId { get; set; }
        
        /// <summary>
        /// Propriedade de navegação para o motel
        /// Permite acesso fácil às informações do motel relacionado
        /// </summary>
        [Required(ErrorMessage = "O quarto deve estar associado a um motel")]
        public Motel Motel { get; set; }
    }
}
