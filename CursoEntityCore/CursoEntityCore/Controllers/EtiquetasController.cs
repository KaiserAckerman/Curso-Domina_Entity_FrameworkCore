using CursoEntityCore.Datos;
using CursoEntityCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CursoEntityCore.Controllers
{
    public class EtiquetasController : Controller
    {
        public readonly ApplicationDbContext _Context;
        public EtiquetasController(ApplicationDbContext contexto)
        {
                _Context = contexto;
        }
        public IActionResult Index()
        {
            List<Etiqueta> listaEtiquetas = _Context.Etiqueta.ToList();
            return View(listaEtiquetas);
        }
        [HttpGet]
        public IActionResult Crear() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Etiqueta etiqueta)
        {
            if (ModelState.IsValid)
            {
                _Context.Etiqueta.Add(etiqueta);
                _Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        
        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)

            {
                return View();
            }
            var etiqueta = _Context.Etiqueta.FirstOrDefault(c => c.Etiqueta_Id == id);
            return View(etiqueta);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Etiqueta etiqueta)
        {
            if (ModelState.IsValid)
            { 
                _Context.Etiqueta.Update(etiqueta);
                _Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        public IActionResult Borrar(int? id)
        {
            var etiqueta = _Context.Etiqueta.FirstOrDefault(c => c.Etiqueta_Id == id);
            _Context.Etiqueta.Remove(etiqueta);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
       
    }
}
