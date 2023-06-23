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
using AsturTravel.Servicios;



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


        //CREA USUARIO DESDE EL BACKEND
        [HttpPost]
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

                    //ENCRIPTAR CONTRASEÑA USUARIO CON BCRYPT
                    usuario.Contrasenas = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenas);
                    usuario.fechaRegistro = DateTime.Now;
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(usuario);
            }
        }

        
        //LOGIN DE USUARIO DESDE EL FRONTED
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
                        //VERIFICAR QUE LA CONTRASEÑA ES COMPATIBLE CON EL HASH ALMACENADO EN LA BASE DE DATOS
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

        //CARGAR VISTA DE EDITAR USUARIO
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

        //EDITAR USUARIO
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

        //BORRAR USUARIO
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

        //OBTENER PROVINCIA A PARTIR DEL DICCIONARIO PROVINCIAS
        public ActionResult GetProvincia(string codProv)
        {

            var values = "";

            foreach (KeyValuePair<string, string> provincia in Utilidades.provincias)
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

        //OBTENER EN JSON TODOS LOS USUARIOS

        [HttpGet("usuario/GetJson")]
        public IActionResult GetJson()
        {
            List<Usuario> listaUsuario = _context.Usuario.ToList();
            return Json(listaUsuario);
        }

        //OBTENER UN USUARIO SEGÚN SU ID
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

        //CARGAR VISTA DE INDEX USUARIOS
        public IActionResult PartialIndex()
        {
            var usuarios = _context.Usuario.ToList();

            return PartialView("PartialsHomeAdmin/Usuarios/_PartialUsuarios", usuarios);
        }

        //CARGAR VISTA DE CREAR USUARIOS
        public IActionResult PartialCreate()
        {
            return PartialView("PartialsHomeAdmin/Usuarios/_PartialCreateUsuarios");
        }
    }
}
