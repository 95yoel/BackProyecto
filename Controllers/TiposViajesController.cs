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

       

        // CARGAR VISTA PARCIAL DE CREAR TIPOS DE VIAJES
        
        public IActionResult Create()
        {
            return PartialView("PartialsHomeAdmin/TiposViajes/_PartialCreateTipos");
        }

        //CREAR TIPO DE VIAJE
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

        // EDITAR TIPO DE VIAJE
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
            return PartialView("PartialsHomeAdmin/TiposViajes/_PartialEditarTipos", tiposViaje);
        }
        

        //EDITAR TIPO DE VIAJE 
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

       //BORRAR TIPO DE VIAJE
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

        //OBTENER UN JSON CON TODOS LOS TIPOS DE VIAJE
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

        //CARGAR INDEX DE TIPOS DE VIAJE
        public IActionResult PartialIndex()
        {
            var tipos = _context.TiposViaje.ToList();

            return PartialView("PartialsHomeAdmin/TiposViajes/_PartialTipos", tipos);
        }
        //CARGAR VISTA CREAR TIPOS DE VIAJE
        public IActionResult PartialCreate()
        {
            return PartialView("PartialsHomeAdmin/TiposViajes/_PartialCreateTipos");
        }


    }
}
