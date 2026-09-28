using System.Collections.Generic;
using System.Threading.Tasks;
using PontoTuristicoApp.Models.DTOs;

namespace PontoTuristicoApp.Services
{
    public interface IIbgeService
    {
        Task<IEnumerable<EstadoIbgeDto>> ObterEstadosAsync();
        Task<IEnumerable<MunicipioIbgeDto>> ObterMunicipiosPorEstadoAsync(int estadoId);
    }
}
