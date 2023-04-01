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
    public class ViajesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ViajesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Viajes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Viajes.Include(v => v.Destino).Include(v => v.TipoViaje);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Viajes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Viajes == null)
            {
                return NotFound();
            }

            var viajes = await _context.Viajes
                .Include(v => v.Destino)
                .Include(v => v.TipoViaje)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viajes == null)
            {
                return NotFound();
            }

            return View(viajes);
        }

        // GET: Viajes/Create
        public IActionResult Create()
        {
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Id");
            ViewData["TipoViajeId"] = new SelectList(_context.TiposViaje, "Id", "Id");
            return View();
        }

        // POST: Viajes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,DestinoId,TipoViajeId,FechaSalida,FechaLlegada,Precio")] Viajes viajes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viajes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Id", viajes.DestinoId);
            ViewData["TipoViajeId"] = new SelectList(_context.TiposViaje, "Id", "Id", viajes.TipoViajeId);
            return View(viajes);
        }

        // GET: Viajes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Viajes == null)
            {
                return NotFound();
            }

            var viajes = await _context.Viajes.FindAsync(id);
            if (viajes == null)
            {
                return NotFound();
            }
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Id", viajes.DestinoId);
            ViewData["TipoViajeId"] = new SelectList(_context.TiposViaje, "Id", "Id", viajes.TipoViajeId);
            return View(viajes);
        }

        // POST: Viajes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,DestinoId,TipoViajeId,FechaSalida,FechaLlegada,Precio")] Viajes viajes)
        {
            if (id != viajes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viajes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViajesExists(viajes.Id))
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
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Id", viajes.DestinoId);
            ViewData["TipoViajeId"] = new SelectList(_context.TiposViaje, "Id", "Id", viajes.TipoViajeId);
            return View(viajes);
        }

        // GET: Viajes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Viajes == null)
            {
                return NotFound();
            }

            var viajes = await _context.Viajes
                .Include(v => v.Destino)
                .Include(v => v.TipoViaje)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viajes == null)
            {
                return NotFound();
            }

            return View(viajes);
        }

        // POST: Viajes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Viajes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Viajes'  is null.");
            }
            var viajes = await _context.Viajes.FindAsync(id);
            if (viajes != null)
            {
                _context.Viajes.Remove(viajes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViajesExists(int id)
        {
          return _context.Viajes.Any(e => e.Id == id);
        }

        public IActionResult GetJson()
        {
            List<Viajes> listaViajes = _context.Viajes.ToList();
            return Json(listaViajes);
        }



        public IActionResult GetViajesPorCliente(int idCliente)
        {
            var viajesPorCliente = _context.Reservas
         .Include(r => r.Viaje)
             .ThenInclude(v => v.Destino)
         .Include(r => r.Viaje)
             .ThenInclude(v => v.TipoViaje)
         .Where(r => r.UsuarioId == idCliente)
         .Select(r => r.Viaje)
         .ToList();

            return Json(viajesPorCliente);
        }



    }
}
