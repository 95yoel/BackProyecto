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
using BCrypt.Net;

namespace AsturTravel.Controllers
{

    [EnableCors("AllowAllOrigins")]
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public string valorNulo = "";
        public Dictionary<string, string> provincias = new Dictionary<string, string>()
        {
            {"01", "Álava"},
            {"02", "Albacete"},
            {"03", "Alicante"},
            {"04", "Almería"},
            {"05", "Ávila"},
            {"06", "Badajoz"},
            {"07", "Islas Baleares"},
            {"08", "Barcelona"},
            {"09", "Burgos"},
            {"10", "Cáceres"},
            {"11", "Cádiz"},
            {"12", "Castellón"},
            {"13", "Ciudad Real"},
            {"14", "Córdoba"},
            {"15", "La Coruña"},
            {"16", "Cuenca"},
            {"17", "Gerona"},
            {"18", "Granada"},
            {"19", "Guadalajara"},
            {"20", "Guipúzcoa"},
            {"21", "Huelva"},
            {"22", "Huesca"},
            {"23", "Jaén"},
            {"24", "León"},
            {"25", "Lérida"},
            {"26", "La Rioja"},
            {"27", "Lugo"},
            {"28", "Madrid"},
            {"29", "Málaga"},
            {"30", "Murcia"},
            {"31", "Navarra"},
            {"32", "Orense"},
            {"33", "Asturias"},
            {"34", "Palencia"},
            {"35", "Las Palmas"},
            {"36", "Pontevedra"},
            {"37", "Salamanca"},
            {"38", "Santa Cruz de Tenerife"},
            {"39", "Cantabria"},
            {"40", "Segovia"},
            {"41", "Sevilla"},
            {"42", "Soria"},
            {"43", "Tarragona"},
            {"44", "Teruel"},
            {"45", "Toledo"},
            {"46", "Valencia"},
            {"47", "Valladolid"},
            {"48", "Vizcaya"},
            {"49", "Zamora"},
            {"50", "Zaragoza"},
            {"51", "Ceuta"},
            {"52", "Melilla"}
        };
        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
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
                usuario.fechaRegistro = DateTime.Now;
                usuario.Contrasenas = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenas);
                _context.Add(usuario);
               await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
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

                    usuario.Rol = Roles.Usuario;
                    usuario.Contrasenas = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenas);
                    usuario.fechaRegistro = DateTime.Now;
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(usuario);
            }
        }

        //public async Task<bool> Login()
        //{
        //    using (var reader = new StreamReader(Request.Body))
        //    {
        //        var requestBody = await reader.ReadToEndAsync();
        //        // Realizar la deserialización del cuerpo de la solicitud a un objeto Usuario
        //        var usuario = JsonConvert.DeserializeObject<Usuario>(requestBody);
        //        if (ModelState.IsValid)
        //        {
        //            var usuarioBD = _context.Usuario.Where(u => u.Email == usuario.Email).FirstOrDefault();
        //            if (usuarioBD != null)
        //            {
        //                if (BCrypt.Net.BCrypt.Verify(usuario.Contrasenas, usuarioBD.Contrasenas))
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //        return false;
        //    }
        //}


        //login alternativo que devuelve el rol del usuario y ademas el bool de si se ha logeado o no

        public async Task<IActionResult> Login()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var requestBody = await reader.ReadToEndAsync();
                // Realizar la deserialización del cuerpo de la solicitud a un objeto Usuario
                var usuario = JsonConvert.DeserializeObject<Usuario>(requestBody);
                if (ModelState.IsValid)
                {
                    var usuarioBD = _context.Usuario.Where(u => u.Email == usuario.Email).FirstOrDefault();
                    if (usuarioBD != null)
                    {
                        if (BCrypt.Net.BCrypt.Verify(usuario.Contrasenas, usuarioBD.Contrasenas))
                        {
                            var rolUsuario = usuarioBD.Rol;
                            var id = usuarioBD.Id;

                            return Ok(new { rol = usuarioBD.Rol, logeado = true ,id = id });
                        }
                        else
                        {

                        }
                    }
                }
                return Ok(new { rol = "", logeado = false,id = "" });
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
            return PartialView("PartialsHomeAdmin/Usuarios/_PartialEditarUsuarios",usuario);
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
                    usuario.fechaRegistro = DateTime.Now;
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
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
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

            return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Delete2(int id)
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
            return RedirectToAction("Index", "Home");
        }

        public ActionResult GetProvincia(string codProv)
        {

            var values = "";

            foreach (KeyValuePair<string, string> provincia in provincias)
            {
                if (provincia.Key == codProv)
                {
                    values = provincia.Value;
                }
            }

            return Json(values);

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

            return PartialView("PartialsHomeAdmin/Usuarios/_PartialUsuarios", usuarios);
        }
        public IActionResult PartialCreate()
        {
            return PartialView("PartialsHomeAdmin/Usuarios/_PartialCreateUsuarios");
        }






    }
}
