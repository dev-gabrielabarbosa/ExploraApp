using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PontoTuristicoApp.Data;
using PontoTuristicoApp.Models;
using PontoTuristicoApp.Models.ViewModels;
using PontoTuristicoApp.Services;

namespace PontoTuristicoApp.Controllers
{
    public class PontosTuristicosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IIbgeService _ibgeService;

        public PontosTuristicosController(AppDbContext context, IIbgeService ibgeService)
        {
            _context = context;
            _ibgeService = ibgeService;
        }

        // GET: PontosTuristicos
        public async Task<IActionResult> Index(string busca, int pagina = 1)
        {
            int itensPorPagina = 5;
            var query = _context.PontosTuristicos.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                var termo = busca.Trim().ToLower();

                // Busca lista de estados do IBGE para permitir buscar por Sigla ou Nome
                var ibgeEstados = await _ibgeService.ObterEstadosAsync();
                var termosEstado = ibgeEstados
                    .Where(e => e.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                                e.Sigla.Equals(termo, StringComparison.OrdinalIgnoreCase))
                    .SelectMany(e => new[] { e.Nome, e.Sigla })
                    .Distinct()
                    .Select(t => t.ToLower())
                    .ToList();

                query = query.Where(p =>
                    EF.Functions.Like(p.Nome, $"%{termo}%") ||
                    EF.Functions.Like(p.Cidade, $"%{termo}%") ||
                    EF.Functions.Like(p.Estado, $"%{termo}%") ||
                    EF.Functions.Like(p.Descricao, $"%{termo}%") ||
                    EF.Functions.Like(p.Localizacao, $"%{termo}%") ||
                    p.Nome.ToLower().Contains(termo) ||
                    p.Cidade.ToLower().Contains(termo) ||
                    p.Estado.ToLower().Contains(termo) ||
                    termosEstado.Contains(p.Estado.ToLower()) ||
                    p.Descricao.ToLower().Contains(termo) ||
                    p.Localizacao.ToLower().Contains(termo));
            }

            var totalItens = await query.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalItens / (double)itensPorPagina);
            if (pagina > totalPaginas && totalPaginas > 0) pagina = totalPaginas;
            if (pagina < 1) pagina = 1;

            var pontos = await query
                .OrderByDescending(p => p.DataInclusao)
                .Skip((pagina - 1) * itensPorPagina)
                .Take(itensPorPagina)
                .ToListAsync();

            ViewBag.Busca = busca;
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(pontos);
        }

        // GET: PontosTuristicos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var ponto = await _context.PontosTuristicos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ponto == null) return NotFound();

            return View(ponto);
        }

        // GET: PontosTuristicos/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Estados = await _ibgeService.ObterEstadosAsync();
            return View(new PontoTuristicoCreateViewModel());
        }

        // GET: PontosTuristicos/MunicipiosPorEstado/35
        [HttpGet]
        public async Task<IActionResult> MunicipiosPorEstado(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { erro = "ID do estado inválido." });
            }

            try
            {
                var municipios = await _ibgeService.ObterMunicipiosPorEstadoAsync(id);
                if (municipios == null || !municipios.Any())
                {
                    return NotFound(new { erro = "Nenhum município encontrado para o estado informado." });
                }

                return Json(municipios.Select(m => new { id = m.Id, nome = m.Nome }));
            }
            catch (Exception)
            {
                return StatusCode(500, new { erro = "Não foi possível carregar as cidades. Tente novamente." });
            }
        }

        // POST: PontosTuristicos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PontoTuristicoCreateViewModel model)
        {
            var estados = await _ibgeService.ObterEstadosAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.Estados = estados;
                if (model.EstadoId.HasValue)
                {
                    ViewBag.Municipios = await _ibgeService.ObterMunicipiosPorEstadoAsync(model.EstadoId.Value);
                }
                return View(model);
            }

            var estadoSelecionado = estados.FirstOrDefault(e => e.Id == model.EstadoId!.Value);
            if (estadoSelecionado == null)
            {
                ModelState.AddModelError("EstadoId", "O estado selecionado não foi encontrado na base do IBGE.");
                ViewBag.Estados = estados;
                return View(model);
            }

            var municipios = await _ibgeService.ObterMunicipiosPorEstadoAsync(model.EstadoId!.Value);
            if (!municipios.Any(m => m.Nome.Equals(model.Cidade, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Cidade", "A cidade selecionada não pertence ao estado selecionado.");
                ViewBag.Estados = estados;
                ViewBag.Municipios = municipios;
                return View(model);
            }

            var pontoTuristico = new PontoTuristico
            {
                Id = Guid.NewGuid(),
                Nome = model.Nome.Trim(),
                Descricao = model.Descricao.Trim(),
                Localizacao = model.Localizacao.Trim(),
                Estado = estadoSelecionado.Sigla,
                Cidade = model.Cidade.Trim(),
                DataInclusao = DateTime.Now
            };

            _context.Add(pontoTuristico);
            await _context.SaveChangesAsync();

            TempData["MensagemSucesso"] = "Ponto turístico cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
