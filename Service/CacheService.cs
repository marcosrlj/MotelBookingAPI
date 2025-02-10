using System.Collections.Concurrent;

namespace MotelBooking.Api.Services
{
    /// <summary>
    /// Serviço responsável pelo gerenciamento de cache em memória da aplicação
    /// Utiliza ConcurrentDictionary para thread-safety em ambientes multi-thread
    /// </summary>
    public class CacheService
    {
        /// <summary>
        /// Dicionário thread-safe para armazenar os dados em cache
        /// Chave: string identificadora do item
        /// Valor: objeto armazenado em cache
        /// </summary>
        private readonly ConcurrentDictionary<string, object> _cache = new();

        /// <summary>
        /// Adiciona ou atualiza um item no cache
        /// </summary>
        /// <param name="key">Chave única para identificar o item</param>
        /// <param name="value">Valor a ser armazenado em cache</param>
        public void Set(string key, object value) => _cache[key] = value;

        /// <summary>
        /// Recupera um item do cache
        /// </summary>
        /// <param name="key">Chave do item a ser recuperado</param>
        /// <returns>O objeto armazenado ou null se não encontrado</returns>
        public object Get(string key) => _cache.TryGetValue(key, out var value) ? value : null;

        /// <summary>
        /// Remove um item do cache
        /// </summary>
        /// <param name="key">Chave do item a ser removido</param>
        /// <returns>True se o item foi removido, False se não existia</returns>
        public bool Remove(string key) => _cache.TryRemove(key, out _);

        /// <summary>
        /// Limpa todo o cache
        /// </summary>
        public void Clear() => _cache.Clear();
    }
}