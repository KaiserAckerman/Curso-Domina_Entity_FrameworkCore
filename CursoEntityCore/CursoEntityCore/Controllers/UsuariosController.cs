using CursoEntityCore.Datos;
using CursoEntityCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CursoEntityCore.Controllers
{
    public class UsuariosController : Controller
    {
        public readonly ApplicationDbContext _Context;
        public UsuariosController(ApplicationDbContext contexto)
        {
            _Context = contexto;
        }
        public IActionResult Index()
        {
            List<Usuario> listaUsuarios = _Context.Usuario.ToList();
            return View(listaUsuarios);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _Context.Usuario.Add(usuario);
                _Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)

            {
                return View();
            }
            var usuario = _Context.Usuario.FirstOrDefault(c => c.Id == id);
            return View(usuario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _Context.Usuario.Update(usuario);
                _Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        public IActionResult Borrar(int? id)
        {
            var usuario = _Context.Usuario.FirstOrDefault(c => c.Id == id);
            _Context.Usuario.Remove(usuario);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Detalle(int? id)
        {
            if (id == null)
            {
                return View();
            }
            var usuario = _Context.Usuario.Include(d => d.DetalleUsuario).FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult AgregarDetalle(Usuario usuario)
        {
            if (usuario.DetalleUsuario.DetalleUsuario_Id == 0)
            {
                //Creamos los detalles para ese usuario
                _Context.DetalleUsuario.Add(usuario.DetalleUsuario);
                _Context.SaveChanges();

                //Despues de crear el detalle del usuario, obtenemos el usuario de la base de datos y le actualizamos el campo "Detalle Usuario_Id"
                var usuarioBd = _Context.Usuario.FirstOrDefault(u => u.Id == usuario.Id);
                usuarioBd.DetalleUsuario_Id = usuario.DetalleUsuario.DetalleUsuario_Id;
                _Context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
