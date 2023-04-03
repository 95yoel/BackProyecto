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
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservas.Include(r => r.Usuario).Include(r => r.Viaje);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Viaje)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservas == null)
            {
                return NotFound();
            }

            return View(reservas);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Nombre");
            ViewData["ViajeId"] = new SelectList(_context.Viajes, "Id", "Nombre");
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,ViajeId,FechaReserva,FechaPago,FechaCancelacion,FechaModificacion,NumeroPersonas,Precio")] Reservas reservas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "Id", reservas.UsuarioId);
            ViewData["ViajeId"] = new SelectList(_context.Viajes, "Id", "Id", reservas.ViajeId);
            return View(reservas);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas.FindAsync(id);
            if (reservas == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "NombreCompleto", reservas.UsuarioId);
            ViewData["ViajeId"] = new SelectList(_context.Viajes, "Id", "Nombre", reservas.ViajeId);
            return View(reservas);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,ViajeId,FechaReserva,FechaPago,FechaCancelacion,FechaModificacion,NumeroPersonas,Precio")] Reservas reservas)
        {
            if (id != reservas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservasExists(reservas.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "Id", "NombreCompleto", reservas.UsuarioId);
            ViewData["ViajeId"] = new SelectList(_context.Viajes, "Id", "Nombre", reservas.ViajeId);
            return View(reservas);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Viaje)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservas == null)
            {
                return NotFound();
            }

            return View(reservas);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservas'  is null.");
            }
            var reservas = await _context.Reservas.FindAsync(id);
            if (reservas != null)
            {
                _context.Reservas.Remove(reservas);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservasExists(int id)
        {
          return _context.Reservas.Any(e => e.Id == id);
        }
        //A la ruta tiene una s menos
        [HttpGet("reserva/GetJson")]
        public IActionResult GetJson()
        {
            List<Reservas> listaReservas = _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Viaje)
                    .ThenInclude(v => v.Destino)
                .Include(r => r.Viaje)
                    .ThenInclude(v => v.TipoViaje)
       
                .ToList();
            return Json(listaReservas);
        }
        //A la ruta tiene una s menos
        [HttpGet("reserva/reservasUsuario/{id}")]
        public async Task<ActionResult<IEnumerable<Reservas>>> GetReservasByUsuario(int id)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Viaje)
                    .ThenInclude(v => v.Destino)
                .Include(r => r.Viaje)
                    .ThenInclude(v => v.TipoViaje)
                .Where(r => r.UsuarioId == id)
                .ToListAsync();

            if (reservas == null || reservas.Count == 0)
            {
                return Json(new List<Reservas>());
            }

            return reservas;
        }
        [HttpGet("reserva/infoReserva/{id}")]
        public async Task<ActionResult<Reservas>> GetReservaById(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Viaje)
                    .ThenInclude(v => v.Destino)
                .Include(r => r.Viaje)
                    .ThenInclude(v => v.TipoViaje)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
            {
                return Json(new List<Reservas>());
            }

            return reserva;
        }








    }
}
