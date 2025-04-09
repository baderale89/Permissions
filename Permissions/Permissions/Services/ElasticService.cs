using Nest;
using Permissions.Interfaces;
using Permissions.Models;

namespace Permissions.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly IElasticClient _client;
        private readonly ILogger<ElasticsearchService> _logger;

        public ElasticsearchService(IElasticClient client, ILogger<ElasticsearchService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task IndexPermissionAsync(Permission permission)
        {
            try
            {
                var response = await _client.IndexDocumentAsync(permission);
                if (!response.IsValid)
                {
                    _logger.LogError("Failed to index permission {PermissionId}", permission.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error indexing permission in Elasticsearch");
                throw;
            }
        }
    }
}