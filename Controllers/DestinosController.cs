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
        

        // GET: Destinos
        public async Task<IActionResult> Index()
        {
              return View(await _context.Destinos.ToListAsync());
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

            return View(destinos);
        }

        // GET: Destinos/Create
        public IActionResult Create()
        {
            return View();
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
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + destinos.ImagenFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    destinos.Imagen = "/imagenes/" + uniqueFileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await destinos.ImagenFile.CopyToAsync(stream);
                    }
                }

                _context.Add(destinos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(destinos);
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
            return View(destinos);
        }

        // POST: Destinos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Imagen")] Destinos destinos)
        {
            if (id != destinos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(destinos);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(destinos);
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

            return View(destinos);
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
            return RedirectToAction(nameof(Index));
        }

        private bool DestinosExists(int id)
        {
          return _context.Destinos.Any(e => e.Id == id);
        }
    }
}
