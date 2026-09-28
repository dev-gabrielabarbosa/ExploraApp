using System;
using System.Collections.Generic;

namespace PontoTuristicoApp.Helpers
{
    public static class EstadoHelper
    {
        private static readonly Dictionary<string, string> EstadoParaUf = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Acre", "AC" }, { "AC", "AC" },
            { "Alagoas", "AL" }, { "AL", "AL" },
            { "Amapá", "AP" }, { "Amapa", "AP" }, { "AP", "AP" },
            { "Amazonas", "AM" }, { "AM", "AM" },
            { "Bahia", "BA" }, { "BA", "BA" },
            { "Ceará", "CE" }, { "Ceara", "CE" }, { "CE", "CE" },
            { "Distrito Federal", "DF" }, { "DF", "DF" },
            { "Espírito Santo", "ES" }, { "Espirito Santo", "ES" }, { "ES", "ES" },
            { "Goiás", "GO" }, { "Goias", "GO" }, { "GO", "GO" },
            { "Maranhão", "MA" }, { "Maranhao", "MA" }, { "MA", "MA" },
            { "Mato Grosso", "MT" }, { "MT", "MT" },
            { "Mato Grosso do Sul", "MS" }, { "MS", "MS" },
            { "Minas Gerais", "MG" }, { "MG", "MG" },
            { "Pará", "PA" }, { "Para", "PA" }, { "PA", "PA" },
            { "Paraíba", "PB" }, { "Paraiba", "PB" }, { "PB", "PB" },
            { "Paraná", "PR" }, { "Parana", "PR" }, { "PR", "PR" },
            { "Pernambuco", "PE" }, { "PE", "PE" },
            { "Piauí", "PI" }, { "Piaui", "PI" }, { "PI", "PI" },
            { "Rio de Janeiro", "RJ" }, { "RJ", "RJ" },
            { "Rio Grande do Norte", "RN" }, { "RN", "RN" },
            { "Rio Grande do Sul", "RS" }, { "RS", "RS" },
            { "Rondônia", "RO" }, { "Rondonia", "RO" }, { "RO", "RO" },
            { "Roraima", "RR" }, { "RR", "RR" },
            { "Santa Catarina", "SC" }, { "SC", "SC" },
            { "São Paulo", "SP" }, { "Sao Paulo", "SP" }, { "SP", "SP" },
            { "Sergipe", "SE" }, { "SE", "SE" },
            { "Tocantins", "TO" }, { "TO", "TO" }
        };

        public static string ObterUf(string? estado)
        {
            if (string.IsNullOrWhiteSpace(estado)) return string.Empty;
            var estadoTrim = estado.Trim();
            if (EstadoParaUf.TryGetValue(estadoTrim, out var uf))
            {
                return uf;
            }
            return estadoTrim.Length <= 2 ? estadoTrim.ToUpperInvariant() : estadoTrim;
        }
    }
}
