using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsturTravel.Data;
using AsturTravel.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

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
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Nombre");
            ViewData["TipoViajeId"] = new SelectList(_context.TiposViaje, "Id", "Tipo");
            return PartialView("PartialsHomeAdmin/_PartialCreateViajes");
        }

        // POST: Viajes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //FIXME : he creado un nuevo campo PrecioString para poder crear el precio, pero no se como hacer para que se guarde en la base de datos
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Viajes viajes)
        {
            if (ModelState.IsValid)
            {

                var culture = new CultureInfo("es-ES");
                viajes.Precio = double.Parse(viajes.PrecioString, culture);


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
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Nombre", viajes.DestinoId);
            ViewData["TipoViajeId"] = new SelectList(_context.TiposViaje, "Id", "Tipo", viajes.TipoViajeId);

            return PartialView("PartialsHomeAdmin/_PartialEditarViajes",viajes);
        }

        // POST: Viajes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //FIXME: he creado un nuevo campo PrecioString para poder editar el precio, pero no se como hacer para que se guarde en la base de datos

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id,Viajes viajes)
        {
            if (id != viajes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var culture = new CultureInfo("es-ES");
                    viajes.Precio = double.Parse(viajes.PrecioString, culture);

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
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Nombre", viajes.DestinoId);
            ViewData["TipoViajeId"] = new SelectList(_context.TiposViaje, "Id", "Tipo", viajes.TipoViajeId);
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
        public IActionResult GetPrecio(int id)
        {

            var precio = _context.Viajes.Find(id).Precio;
            var tipo = _context.Viajes.Find(id).TipoViaje;
            var destino = _context.Viajes.Find(id).Destino;

             
            return Json(new { precio, tipo, destino });
        }

        public IActionResult getPrecioViaje(int id)
        {
            var reservas = _context.Reservas.Find(id);
            if (id == -1)
            {
                return Json(0);
            }
            else
            {
                var precio = reservas.Precio;
                return Json(precio);
            }

        }




        //creo que no hace falta ya que hago lo mismo en reservas
        //public IActionResult GetViajesPorCliente(int idCliente)
        //{
        //    var viajesPorCliente = _context.Reservas
        // .Include(r => r.Viaje)
        //     .ThenInclude(v => v.Destino)
        // .Include(r => r.Viaje)
        //     .ThenInclude(v => v.TipoViaje)
        // .Where(r => r.UsuarioId == idCliente)
        // .Select(r => r.Viaje)
        // .ToList();

        //    return Json(viajesPorCliente);
        //}
        public IActionResult PartialIndex()
        {
            var viajes = _context.Viajes.Include(v => v.Destino).Include(v => v.TipoViaje);

            return PartialView("PartialsHomeAdmin/_PartialViajes", viajes);
        }
        public IActionResult PartialCreate()
        {
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "Nombre");
            ViewData["TipoViajeId"] = new SelectList(_context.TiposViaje, "Id", "Tipo");
            return PartialView("PartialsHomeAdmin/_PartialCreateViajes");
        }

    }
}
