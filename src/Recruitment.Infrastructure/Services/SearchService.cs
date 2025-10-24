using Nest;
using Microsoft.Extensions.Configuration;

namespace Recruitment.Infrastructure.Services;

public class SearchService
{
    private readonly IElasticClient _elasticClient;
    private readonly string _indexName;

    public SearchService(IElasticClient elasticClient, IConfiguration configuration)
    {
        _elasticClient = elasticClient;
        _indexName = configuration["Elasticsearch:IndexName"];
    }

    public async Task IndexVacancyAsync(object vacancy)
    {
        var response = await _elasticClient.IndexAsync(vacancy, i => i.Index(_indexName));
        if (!response.IsValid)
        {
            throw new Exception($"Failed to index vacancy: {response.OriginalException.Message}");
        }
    }

    public async Task IndexCandidateAsync(object candidate)
    {
        var response = await _elasticClient.IndexAsync(candidate, i => i.Index(_indexName));
        if (!response.IsValid)
        {
            throw new Exception($"Failed to index candidate: {response.OriginalException.Message}");
        }
    }

    public async Task<List<object>> SearchAsync(string query, int size = 10)
    {
        var searchResponse = await _elasticClient.SearchAsync<object>(s => s
            .Index(_indexName)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(query)
                    .Fields(f => f
                        .Field("title")
                        .Field("description")
                        .Field("skills")
                    )
                )
            )
            .Size(size)
        );

        if (!searchResponse.IsValid)
        {
            throw new Exception($"Search failed: {searchResponse.OriginalException.Message}");
        }

        return searchResponse.Documents.ToList();
    }
}