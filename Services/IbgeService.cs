using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PontoTuristicoApp.Models.DTOs;

namespace PontoTuristicoApp.Services
{
    public class IbgeService : IIbgeService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<IbgeService> _logger;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public IbgeService(HttpClient httpClient, IMemoryCache cache, ILogger<IbgeService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<IEnumerable<EstadoIbgeDto>> ObterEstadosAsync()
        {
            const string cacheKey = "ibge_estados";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<EstadoIbgeDto>? estadosCached) && estadosCached != null)
            {
                return estadosCached;
            }

            try
            {
                var response = await _httpClient.GetAsync("https://servicodados.ibge.gov.br/api/v1/localidades/estados?orderBy=nome");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var estados = JsonSerializer.Deserialize<List<EstadoIbgeDto>>(content, _jsonOptions);

                if (estados != null && estados.Count > 0)
                {
                    var ordenados = estados.OrderBy(e => e.Nome).ToList();
                    _cache.Set(cacheKey, ordenados, TimeSpan.FromHours(1));
                    return ordenados;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar API de estados do IBGE.");
            }

            return Enumerable.Empty<EstadoIbgeDto>();
        }

        public async Task<IEnumerable<MunicipioIbgeDto>> ObterMunicipiosPorEstadoAsync(int estadoId)
        {
            if (estadoId <= 0)
            {
                return Enumerable.Empty<MunicipioIbgeDto>();
            }

            string cacheKey = $"ibge_municipios_{estadoId}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<MunicipioIbgeDto>? municipiosCached) && municipiosCached != null)
            {
                return municipiosCached;
            }

            try
            {
                var url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{estadoId}/municipios?orderBy=nome";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var municipios = JsonSerializer.Deserialize<List<MunicipioIbgeDto>>(content, _jsonOptions);

                if (municipios != null && municipios.Count > 0)
                {
                    var ordenados = municipios.OrderBy(m => m.Nome).ToList();
                    _cache.Set(cacheKey, ordenados, TimeSpan.FromHours(1));
                    return ordenados;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar API de municípios do IBGE para o estado {EstadoId}.", estadoId);
            }

            return Enumerable.Empty<MunicipioIbgeDto>();
        }
    }
}
