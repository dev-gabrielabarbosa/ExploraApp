using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PontoTuristicoApp.Data;
using PontoTuristicoApp.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PontoTuristicoApp.Controllers
{
    public class PontosTuristicosController : Controller
    {
        private readonly AppDbContext _context;

        public PontosTuristicosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PontosTuristicos
        public async Task<IActionResult> Index(string busca, int pagina = 1)
        {
            int itensPorPagina = 5;
            var query = _context.PontosTuristicos.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                var termo = busca.Trim();
                var estadosDict = ObterEstadosBrasileiros();
                var ufsCorrespondentes = estadosDict
                    .Where(e => e.Value.Contains(termo, StringComparison.OrdinalIgnoreCase) || e.Key.Equals(termo, StringComparison.OrdinalIgnoreCase))
                    .Select(e => e.Key)
                    .ToList();

                query = query.Where(p => 
                    p.Nome.Contains(termo) || 
                    p.Cidade.Contains(termo) || 
                    p.Estado.Contains(termo) || 
                    ufsCorrespondentes.Contains(p.Estado) ||
                    p.Descricao.Contains(termo) || 
                    p.Localizacao.Contains(termo));
            }

            var totalItens = await query.CountAsync();
            var totalPaginas = (int)System.Math.Ceiling(totalItens / (double)itensPorPagina);
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
        public IActionResult Create()
        {
            ViewBag.Estados = ObterEstadosBrasileiros();
            return View();
        }

        // POST: PontosTuristicos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Localizacao,Cidade,Estado")] PontoTuristico pontoTuristico)
        {
            if (ModelState.IsValid)
            {
                pontoTuristico.Id = Guid.NewGuid();
                pontoTuristico.DataInclusao = DateTime.Now;
                _context.Add(pontoTuristico);
                await _context.SaveChangesAsync();
                
                TempData["MensagemSucesso"] = "Ponto turístico cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Estados = ObterEstadosBrasileiros();
            return View(pontoTuristico);
        }

        private Dictionary<string, string> ObterEstadosBrasileiros()
        {
            return new Dictionary<string, string>
            {
                { "AC", "AC — Acre" },
                { "AL", "AL — Alagoas" },
                { "AP", "AP — Amapá" },
                { "AM", "AM — Amazonas" },
                { "BA", "BA — Bahia" },
                { "CE", "CE — Ceará" },
                { "DF", "DF — Distrito Federal" },
                { "ES", "ES — Espírito Santo" },
                { "GO", "GO — Goiás" },
                { "MA", "MA — Maranhão" },
                { "MT", "MT — Mato Grosso" },
                { "MS", "MS — Mato Grosso do Sul" },
                { "MG", "MG — Minas Gerais" },
                { "PA", "PA — Pará" },
                { "PB", "PB — Paraíba" },
                { "PR", "PR — Paraná" },
                { "PE", "PE — Pernambuco" },
                { "PI", "PI — Piauí" },
                { "RJ", "RJ — Rio de Janeiro" },
                { "RN", "RN — Rio Grande do Norte" },
                { "RS", "RS — Rio Grande do Sul" },
                { "RO", "RO — Rondônia" },
                { "RR", "RR — Roraima" },
                { "SC", "SC — Santa Catarina" },
                { "SP", "SP — São Paulo" },
                { "SE", "SE — Sergipe" },
                { "TO", "TO — Tocantins" }
            };
        }
    }
}
