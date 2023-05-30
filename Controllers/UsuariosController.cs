using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsturTravel.Data;
using AsturTravel.Models;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

namespace AsturTravel.Controllers
{

    [EnableCors("AllowAllOrigins")]
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public string valorNulo = "";

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
              return View(await _context.Usuario.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //CREA USUARIO DESDE EL BACKEND
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
               _context.Add(usuario);
               await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        [HttpPost]
        //CREA USUARIO DESDE EL FRONTED
        public async Task<IActionResult>Crear()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var requestBody = await reader.ReadToEndAsync();

                // Realizar la deserialización del cuerpo de la solicitud a un objeto Usuario
                var usuario = JsonConvert.DeserializeObject<Usuario>(requestBody);

                if (ModelState.IsValid)
                {
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(usuario);
            }
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([FromBody] Usuario usuario)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            usuario.Rol = Roles.Usuario;
        //            _context.Add(usuario);
        //            await _context.SaveChangesAsync();
        //            return Ok(); // Opcionalmente, puedes devolver una respuesta HTTP 200 OK si la creación es exitosa
        //        }
        //        return BadRequest(ModelState); // Devuelve una respuesta HTTP 400 Bad Request si el modelo no es válido
        //    }
        //    catch (Exception ex)
        //    {
        //        // Maneja cualquier excepción que pueda ocurrir durante el proceso de creación
        //        return StatusCode(500, ex.Message); // Devuelve una respuesta HTTP 500 Internal Server Error con el mensaje de error
        //    }
        //}


        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return PartialView("PartialsHomeAdmin/_PartialEditarUsuarios",usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuario == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Usuario'  is null.");
            }
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return _context.Usuario.Any(e => e.Id == id);
        }


        [HttpGet("usuario/GetJson")]
        public IActionResult GetJson()
        {
            List<Usuario> listaUsuario = _context.Usuario.ToList();
            return Json(listaUsuario);
        }
        [HttpGet("usuario/{id}")]
        public IActionResult InfoUsuario(int id)
        {
            Usuario usuario = _context.Usuario.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return Json(new List<Usuario>());
            }
            return Json(usuario);
        }


        public IActionResult PartialIndex()
        {
            var usuarios = _context.Usuario.ToList();

            return PartialView("PartialsHomeAdmin/_PartialUsuarios", usuarios);
        }
        public IActionResult PartialCreate()
        {
            return PartialView("PartialsHomeAdmin/_PartialCreateUsuarios");
        }






    }
}
