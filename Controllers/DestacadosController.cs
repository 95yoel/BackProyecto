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

        // GET: Destacados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Destacados == null)
            {
                return NotFound();
            }

            var destacados = await _context.Destacados
                .Include(d => d.Viaje)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destacados == null)
            {
                return NotFound();
            }

            return View(destacados);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ViajeId"] = new SelectList(_context.Viajes, "Id", "Nombre", destacados.ViajeId);
            return View(destacados);
        }

        // GET: Destacados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Destacados == null)
            {
                return NotFound();
            }

            var destacados = await _context.Destacados.FindAsync(id);
            if (destacados == null)
            {
                return NotFound();
            }
            ViewData["ViajeId"] = new SelectList(_context.Viajes, "Id", "Nombre", destacados.ViajeId);
            return View(destacados);
        }

        // POST: Destacados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ViajeId")] Destacados destacados)
        {
            if (id != destacados.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(destacados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestacadosExists(destacados.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ViajeId"] = new SelectList(_context.Viajes, "Id", "Nombre", destacados.ViajeId);
            return View(destacados);
        }

        // GET: Destacados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Destacados == null)
            {
                return NotFound();
            }

            var destacados = await _context.Destacados
                .Include(d => d.Viaje)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destacados == null)
            {
                return NotFound();
            }

            return View(destacados);
        }

        // POST: Destacados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
            return RedirectToAction(nameof(Index));
        }

        private bool DestacadosExists(int id)
        {
          return (_context.Destacados?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet("destacado/GetJson")]
        public IActionResult GetJson()
        {
            List<Destacados> listaDestacados = _context.Destacados.ToList();
            return Json(listaDestacados);
        }

    }
}
