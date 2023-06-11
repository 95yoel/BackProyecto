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
    public class TiposViajesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TiposViajesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TiposViajes
        public async Task<IActionResult> Index()
        {
              return View(await _context.TiposViaje.ToListAsync());
        }

        // GET: TiposViajes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TiposViaje == null)
            {
                return NotFound();
            }

            var tiposViaje = await _context.TiposViaje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tiposViaje == null)
            {
                return NotFound();
            }

            return View(tiposViaje);
        }

        // GET: TiposViajes/Create
        
        public IActionResult Create()
        {
            return PartialView("PartialsHomeAdmin/_PartialCreateTipos");
        }

        // POST: TiposViajes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tipo")] TiposViaje tiposViaje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tiposViaje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: TiposViajes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TiposViaje == null)
            {
                return NotFound();
            }

            var tiposViaje = await _context.TiposViaje.FindAsync(id);
            if (tiposViaje == null)
            {
                return NotFound();
            }
            return PartialView("PartialsHomeAdmin/_PartialEditarTipos", tiposViaje);
        }
        

        // POST: TiposViajes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tipo")] TiposViaje tiposViaje)
        {
            if (id != tiposViaje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tiposViaje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TiposViajeExists(tiposViaje.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: TiposViajes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TiposViaje == null)
            {
                return NotFound();
            }

            var tiposViaje = await _context.TiposViaje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tiposViaje == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: TiposViajes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TiposViaje == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TiposViaje'  is null.");
            }
            var tiposViaje = await _context.TiposViaje.FindAsync(id);
            if (tiposViaje != null)
            {
                _context.TiposViaje.Remove(tiposViaje);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Delete2(int id)
        {
            if (_context.TiposViaje == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TiposViaje'  is null.");
            }
            var tiposViaje = await _context.TiposViaje.FindAsync(id);
            if (tiposViaje != null)
            {
                _context.TiposViaje.Remove(tiposViaje);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool TiposViajeExists(int id)
        {
            return _context.TiposViaje.Any(e => e.Id == id);


        }

        //ATENCION RUTAS
        // por algún conflicto no puedo usar la misma ruta de tiposViajes , entonces he quitado el plural

        //esta ruta funciona con tiposViaje
        [HttpGet("tiposViaje/GetJson")]
        public IActionResult GetJson()
        {
            List<TiposViaje> listaTiposViaje = _context.TiposViaje.ToList();
            return Json(listaTiposViaje);
        }
        //y esta tiposViaje
        [HttpGet("tiposViaje/{id}")]
        public async Task<ActionResult<IEnumerable<Viajes>>> GetViajesByTipoViaje(int id)
        {
            var viajes = await _context.Viajes
                .Where(v => v.TipoViajeId == id)
                .ToListAsync();

            if (viajes == null || viajes.Count == 0)
            {
                return Json(new List<Viajes>());
            }

            return viajes;
        }

        public IActionResult PartialIndex()
        {
            var tipos = _context.TiposViaje.ToList();

            return PartialView("PartialsHomeAdmin/_PartialTipos", tipos);
        }

        public IActionResult PartialCreate()
        {
            return PartialView("PartialsHomeAdmin/_PartialCreateTipos");
        }


    }
}
