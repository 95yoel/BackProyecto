using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsturTravel.Data;
using AsturTravel.Models;

namespace AsturTravel.Controllers
{
    public class DestacadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DestacadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Destacados
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Destacados.Include(d => d.Viaje);
            return View(await applicationDbContext.ToListAsync());
        }

        
        // GET: Destacados/Create
        public IActionResult Create()
        {
            ViewData["ViajeId"] = new SelectList(_context.Viajes, "Id", "Nombre");
            return View();
        }

        // POST: Destacados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ViajeId")] Destacados destacados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(destacados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewData["ViajeId"] = new SelectList(_context.Viajes, "Id", "Nombre", destacados.ViajeId);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete2(int id)
        {
            if (_context.Destacados == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Destacados'  is null.");
            }
            var destacados = await _context.Destacados.FindAsync(id);
            if (destacados != null)
            {
                _context.Destacados.Remove(destacados);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool DestacadosExists(int id)
        {
          return (_context.Destacados?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet("destacado/GetJson")]
        public IActionResult GetJson()
        {
            List<Destacados> listaDestacados = _context.Destacados.ToList();
            List<Viajes> viajes = new List<Viajes>();

            foreach(Destacados destacado in listaDestacados)
            {
                int viajeId = destacado.ViajeId;

                Viajes viaje = ObtenerViajePorId(viajeId);

                viajes.Add(viaje);
            }


            return Json(viajes);
        }

        public Viajes ObtenerViajePorId(int viajeId)
        {
            Viajes viaje = _context.Viajes.Find(viajeId);
            return viaje;
        }

    }
}
