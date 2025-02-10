using System;
using System.ComponentModel.DataAnnotations;

namespace MotelBooking.Api.Models
{
    /// <summary>
    /// Modelo que representa um motel no sistema
    /// Contém as informações básicas de identificação e localização
    /// </summary>
    public class Motel
    {
        /// <summary>
        /// Identificador único do motel
        /// Gerado automaticamente pelo banco de dados
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Endereço completo do motel
        /// Campo obrigatório usando o modificador required do C# 11
        /// </summary>
        [Required(ErrorMessage = "O endereço é obrigatório")]
        public required string Endereco { get; set; }

        /// <summary>
        /// Nome comercial do motel
        /// Campo obrigatório usando o modificador required do C# 11
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório")]
        public required string Nome { get; set; }

        /// <summary>
        /// Localização geográfica do motel (coordenadas ou região)
        /// Campo obrigatório usando o modificador required do C# 11
        /// </summary>
        [Required(ErrorMessage = "A localização é obrigatória")]
        public required string Localizacao { get; set; }
    }
}