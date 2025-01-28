using CursoEntityCore.Datos;
using CursoEntityCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CursoEntityCore.Controllers
{
    public class CategoriasController : Controller
    {
        public readonly ApplicationDbContext _Context;
        public CategoriasController(ApplicationDbContext contexto)
        {
                _Context = contexto;
        }
        public IActionResult Index()
        {
            //Consulta inicial
            //List<Categoria> listaCategorias = _Context.Categoria.ToList();

            //Consulta filtrando por fecha
            //DateTime fechaComparacion = new DateTime(2025, 11, 05);
            //List<Categoria> listaCategorias = _Context.Categoria.Where(f=>f.FechaCreacion >= fechaComparacion).OrderByDescending(f => f.FechaCreacion).ToList();
            //return View(listaCategorias);

            //Seleccionar columnas especificas
            //List<Categoria> listaCategorias = _Context.Categoria.ToList();

            //Agrupar
            //var listaCategorias = _Context.Categoria
            //    .GroupBy(c => new { c.Activo })
            //    .Select(c => new { c.Key, Count = c.Count() }).ToList();

            //Consultas SQL Convencionales
            //List<Categoria> listaCategorias = _Context.Categoria.FromSqlRaw("select * from Categoria where nombre like 'categoria%' and Activo = 1").ToList();

            //Interpolacion de string (string interpolation)
            //var id = 91;
            //var categoria = _Context.Categoria.FromSqlRaw($"select * from categoria where categoria_id = {id}").ToList();
            List<Categoria> listaCategorias = _Context.Categoria.ToList();
            return View(listaCategorias);
        }
        [HttpGet]
        public IActionResult Crear() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _Context.Categoria.Add(categoria);
                _Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult CrearMultipleOpcion2()
        {
            List<Categoria> categorias = new List<Categoria>();    
            for (int i = 0; i < 2; i++)
            {
                categorias.Add(new Categoria { Nombre = Guid.NewGuid().ToString() });
                // _Context.Categoria.Add(new Categoria { Nombre = Guid.NewGuid().ToString() });
            }
            _Context.Categoria.AddRange(categorias);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult CrearMultipleOpcion5()
        {
            List<Categoria> categorias = new List<Categoria>();
            for (int i = 0; i < 5; i++)
            {
                categorias.Add(new Categoria { Nombre = Guid.NewGuid().ToString() });
                //_Context.Categoria.Add(new Categoria { Nombre = Guid.NewGuid().ToString() });
            }
            _Context.Categoria.AddRange(categorias);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
         public IActionResult VistaCrearMultipleOpcionFormulario()
            {
                return View();
            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearMultipleOpcionFormulario()
        {
            string categoriasForm = Request.Form["Nombre"];
            var listaCategorias = from val in categoriasForm.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries) select(val);
            List<Categoria> categorias = new List<Categoria>();
            foreach (var categoria in listaCategorias) { 
            categorias.Add(new Categoria
                    {
                Nombre = categoria
                
                });
            }
            _Context.Categoria.AddRange(categorias);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)

            {
                return View();
            }
            var categoria = _Context.Categoria.FirstOrDefault(c => c.Categoria_Id == id);
            return View(categoria);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Categoria categoria)
        {
            if (ModelState.IsValid)
            { 
                _Context.Categoria.Update(categoria);
                _Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        public IActionResult Borrar(int? id)
        {
            var categoria = _Context.Categoria.FirstOrDefault(c => c.Categoria_Id == id);
            _Context.Categoria.Remove(categoria);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult BorrarMultiple2()
        {
            IEnumerable<Categoria> categorias = _Context.Categoria.OrderByDescending(c => c.Categoria_Id).Take(2);
            _Context.Categoria.RemoveRange(categorias);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult BorrarMultiple5()
        {
            IEnumerable<Categoria> categorias = _Context.Categoria.OrderByDescending(c => c.Categoria_Id).Take(5);
            _Context.Categoria.RemoveRange(categorias);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
