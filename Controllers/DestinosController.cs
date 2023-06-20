using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsturTravel.Data;
using AsturTravel.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace AsturTravel.Controllers
{
    public class DestinosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public DestinosController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        

        // GET: Destinos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Destinos == null)
            {
                return NotFound();
            }

            var destinos = await _context.Destinos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destinos == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Destinos/Create
        public IActionResult Create()
        {
            return PartialView("PartialsHomeAdmin/_PartialCreateDestinos");
        }

        // POST: Destinos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Destinos destinos)
        {
            if (ModelState.IsValid)
            {
                if (destinos.ImagenFile != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "imagenes");
                    string uniqueFileName = /*Guid.NewGuid().ToString() + "_" +*/ destinos.ImagenFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    destinos.Imagen = "https://localhost:7227/imagenes/" + uniqueFileName;
                    ViewBag.imagen = destinos.Imagen;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await destinos.ImagenFile.CopyToAsync(stream);
                    }
                }

                _context.Add(destinos);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Destinos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Destinos == null)
            {
                return NotFound();
            }

            var destinos = await _context.Destinos.FindAsync(id);
            if (destinos == null)
            {
                return NotFound();
            }
            return PartialView("PartialsHomeAdmin/_PartialEditarDestinos",destinos);
        }

        // POST: Destinos/Edit/5
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Destinos destinos)
        {
            if (id != destinos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (destinos.ImagenFile != null)
                    {
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "imagenes");
                        string uniqueFileName = destinos.ImagenFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        destinos.Imagen = "https://localhost:7227/imagenes/" + uniqueFileName;
                        ViewBag.imagen = destinos.Imagen;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await destinos.ImagenFile.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        destinos.Imagen = GetImage(destinos.Id);
                    }

                    var existingDestinos = await _context.Destinos.FindAsync(id);
                    if (existingDestinos != null)
                    {
                        existingDestinos.Nombre = destinos.Nombre;
                        existingDestinos.Descripcion = destinos.Descripcion;
                        existingDestinos.Imagen = destinos.Imagen;

                        _context.Update(existingDestinos);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestinosExists(destinos.Id))
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


        // GET: Destinos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Destinos == null)
            {
                return NotFound();
            }

            var destinos = await _context.Destinos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destinos == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: Destinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Destinos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Destinos'  is null.");
            }
            var destinos = await _context.Destinos.FindAsync(id);
            if (destinos != null)
            {
                _context.Destinos.Remove(destinos);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Home","Index");
        }


        public async Task<IActionResult> Delete2(int id)
        {
            if (_context.Destinos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Destinos'  is null.");
            }
            var destinos = await _context.Destinos.FindAsync(id);
            if (destinos != null)
            {
                _context.Destinos.Remove(destinos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }


        private bool DestinosExists(int id)
        {
          return _context.Destinos.Any(e => e.Id == id);
        }

        [HttpGet("destino/GetJson")]
        public IActionResult GetJson()
        {
            List<Destinos> listaDestinos = _context.Destinos.ToList();
            return Json(listaDestinos);
        }

        [HttpGet("destino/viajesDestino/{id}")]
        public async Task<ActionResult<IEnumerable<Viajes>>> GetViajesByDestino(int id)
        {
            var viajes = await _context.Viajes
                .Where(v => v.DestinoId == id)
                .ToListAsync();

            if (viajes == null || viajes.Count == 0)
            {
                return Json(new List<Destinos>());
            }

            return viajes;
        }
        [HttpGet("destino/infoDestino/{id}")]
        public async Task<ActionResult<Destinos>> GetDestinoById(int id)
        {
            var destino = await _context.Destinos.FindAsync(id);

            if (destino == null)
            {
                return Json(new List<Destinos>());
            }

            return destino;
        }
        public IActionResult PartialIndex()
        {
            var destinos = _context.Destinos.ToList();

            return PartialView("PartialsHomeAdmin/_PartialDestinos",destinos);
        }


        public String GetImage(int id)
        {
            return _context.Destinos.Find(id).Imagen;
        }
    }
}
