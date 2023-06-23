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
        
        // CARGAR VISTA DE CREAR DESTINOS
        public IActionResult Create()
        {
            return PartialView("PartialsHomeAdmin/Destinos/_PartialCreateDestinos");
        }

        //CREAR DESTINO
        [HttpPost]
        public async Task<IActionResult> Create(Destinos destinos)
        {
            if (ModelState.IsValid)
            {
                if (destinos.ImagenFile != null)
                {
                    //ELEGIR CARPETA DONDE SE GUARDARAN LAS IMAGENES
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "imagenes");
                    //ELEGIR NOMBRE DEL ARCHIVO
                    string uniqueFileName =  destinos.ImagenFile.FileName;
                    //UNIR CARPETA Y NOMBRE DEL ARCHIVO
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    //GUARDAR LA RUTA DE LA IMAGEN EN LA BASE DE DATOS
                    destinos.Imagen = "https://localhost:7227/imagenes/" + uniqueFileName;
                    ViewBag.imagen = destinos.Imagen;
                    //GUARDAR LA IMAGEN EN LA CARPETA
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await destinos.ImagenFile.CopyToAsync(stream);
                    }
                }
                //AÑADIR DESTINO
                _context.Add(destinos);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // CARGAR VISTA EDITAR DESTINO
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
            return PartialView("PartialsHomeAdmin/Destinos/_PartialEditarDestinos", destinos);
        }

        // EDITAR DESTINO
        
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
                        //ELEGIR CARPETA DONDE SE GUARDARAN LAS IMAGENES
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "imagenes");
                        //ELEGIR NOMBRE DEL ARCHIVO
                        string uniqueFileName = destinos.ImagenFile.FileName;
                        //UNIR CARPETA Y NOMBRE DEL ARCHIVO
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        //GUARDAR LA RUTA DE LA IMAGEN EN LA BASE DE DATOS
                        destinos.Imagen = "https://localhost:7227/imagenes/" + uniqueFileName;
                        ViewBag.imagen = destinos.Imagen;
                        //GUARDAR LA IMAGEN EN LA CARPETA
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

        //BORRAR DESTINO
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


        //OBTENER JSON DE DESTINOS
        [HttpGet("destino/GetJson")]
        public IActionResult GetJson()
        {
            List<Destinos> listaDestinos = _context.Destinos.ToList();
            return Json(listaDestinos);
        }

        //OBTENER LOS VIAJES QUE PERTECEN A UN DESTINO
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

        //OBTEENER DESTINO POR ID
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
        //CARGAR VISTA PARCIAL INDEX DE DESTINOS
        public IActionResult PartialIndex()
        {
            var destinos = _context.Destinos.ToList();

            return PartialView("PartialsHomeAdmin/Destinos/_PartialDestinos",destinos);
        }

        //OBTENER IMAGEN DE UN DESTINO EN CONCRETO
        public String GetImage(int id)
        {
            return _context.Destinos.Find(id).Imagen;
        }
    }
}
