namespace MotelBooking.Api.Settings
{
    /// <summary>
    /// Classe para configuração das opções do JWT (JSON Web Token)
    /// Mapeada do appsettings.json para uso na aplicação
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Chave secreta usada para assinar os tokens
        /// Deve ser uma string segura e complexa
        /// Recomendado: mínimo 256 bits (32 caracteres)
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Emissor do token (quem está gerando)
        /// Geralmente o nome ou URL da aplicação
        /// Exemplo: "MotelBookingApi"
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Audiência do token (para quem é destinado)
        /// Pode ser o mesmo que o Issuer ou um cliente específico
        /// Exemplo: "https://api.motelbooking.com"
        /// </summary>
        public string Audience { get; set; } = string.Empty;
    }
}