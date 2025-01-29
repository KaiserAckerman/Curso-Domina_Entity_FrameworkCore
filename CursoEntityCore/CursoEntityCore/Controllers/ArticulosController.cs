using CursoEntityCore.Datos;
using CursoEntityCore.Models;
using CursoEntityCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CursoEntityCore.Controllers
{
    public class ArticulosController : Controller
    {
        public readonly ApplicationDbContext _Context;

        public ArticulosController(ApplicationDbContext contexto)
        {
            _Context = contexto;
        }
        public IActionResult Index()
        {
            //Opcion 1 sin datos relacionados(solo trae el ID de la categoria)
            //List<Articulo> listaArticulos = _Context.Articulo.ToList();
            //foreach (var articulo in listaArticulos)
            //{
            //    //Opcion 2: Carga manual se generan muchas consultas SQL, NO ES MUY EFICIENTE si necesitamos cargar mas propiedades
            //    //articulo.Categoria = _Context.Categoria.FirstOrDefault(c => c.Categoria_Id == articulo.Categoria_Id);

            //    //Opcion 3: Carga Explicita (Explicit Loading)
            //    //_Context.Entry(articulo).Reference(c=>c.Categoria).Load();

            //    

            //}

            //Opcion 4: Carga Diligente(Eager Loading)
            List<Articulo> listaArticulos = _Context.Articulo.Include(c => c.Categoria).ToList();
            return View(listaArticulos);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ArticuloCategoriaVM articuloCategorias = new ArticuloCategoriaVM();
            articuloCategorias.ListaCategorias = _Context.Categoria.Select(i => new SelectListItem
            {
                Text = i.Nombre,
                Value = i.Categoria_Id.ToString()
            });
            return View(articuloCategorias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                _Context.Articulo.Add(articulo);
                _Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            //Para retornar la vista por algun error, tambien retorne la lista de categorias
            ArticuloCategoriaVM articuloCategorias = new ArticuloCategoriaVM();
            articuloCategorias.ListaCategorias = _Context.Categoria.Select(i => new SelectListItem
            {
                Text = i.Nombre,
                Value = i.Categoria_Id.ToString()
            });
            return View(articuloCategorias);

        }
        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)

            {
                return View();
            }
            //Para retornar la vista por algun error, tambien retorne la lista de categorias
            ArticuloCategoriaVM articuloCategorias = new ArticuloCategoriaVM();
            articuloCategorias.ListaCategorias = _Context.Categoria.Select(i => new SelectListItem
            {
                Text = i.Nombre,
                Value = i.Categoria_Id.ToString()
            });
            articuloCategorias.Articulo = _Context.Articulo.FirstOrDefault(c => c.Articulo_Id == id);
            if (articuloCategorias == null)

            {
                return NotFound();
            }
            return View(articuloCategorias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(ArticuloCategoriaVM articuloVM)
        {
            if (articuloVM.Articulo.Articulo_Id == 0)
            {
                return View(articuloVM.Articulo);
            }
            else
            {
                _Context.Articulo.Update(articuloVM.Articulo);
                _Context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public IActionResult Borrar(int? id)
        {
            var articulo = _Context.Articulo.FirstOrDefault(c => c.Articulo_Id == id);
            _Context.Articulo.Remove(articulo);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult AdministrarEtiquetas(int id)
        {
            ArticuloEtiquetaVM articuloEtiquetas = new ArticuloEtiquetaVM
            {
                ListaArticuloEtiquetas = _Context.ArticuloEtiqueta.Include(e => e.Etiqueta).Include(a => a.Articulo)
                .Where(a => a.Articulo_Id == id),

                ArticuloEtiqueta = new ArticuloEtiqueta()
                {
                    Articulo_Id = id
                },
                Articulo = _Context.Articulo.FirstOrDefault(a=>a.Articulo_Id == id)
            };

            List<int> listaTemporalEtiquetasArticulo = articuloEtiquetas.ListaArticuloEtiquetas.Select(e => e.Etiqueta_Id).ToList();
            //Obtener todas las etiquetas cuyos id's no esten en la  listaTemporalEtiquetasArticulo
            //Crear un NOT IN usando LINQ
            var listaTemporal = _Context.Etiqueta.Where(e=> !listaTemporalEtiquetasArticulo.Contains(e.Etiqueta_Id)).ToList();

            //Crear lista de etiquetas para el dropdown
            articuloEtiquetas.ListaEtiquetas = listaTemporal.Select(i => new SelectListItem
            {
                Text = i.Titulo,
                Value = i.Etiqueta_Id.ToString()
            });

            return View(articuloEtiquetas);
        }

        [HttpPost]
        public IActionResult AdministrarEtiquetas(ArticuloEtiquetaVM articuloEtiquetas)
        {
            if (articuloEtiquetas.ArticuloEtiqueta.Articulo_Id !=0 && articuloEtiquetas.ArticuloEtiqueta.Etiqueta_Id != 0)
            {
                _Context.ArticuloEtiqueta.Add(articuloEtiquetas.ArticuloEtiqueta);
                _Context.SaveChanges();
            }
            return RedirectToAction(nameof(AdministrarEtiquetas), new
            {
                @id = articuloEtiquetas.ArticuloEtiqueta.Articulo_Id
            });
        }


        [HttpPost]
        public IActionResult EliminarEtiquetas(int idEtiqueta, ArticuloEtiquetaVM articuloEtiquetas)
        {
            int idArticulo = articuloEtiquetas.Articulo.Articulo_Id;
            ArticuloEtiqueta articuloEtiqueta = _Context.ArticuloEtiqueta.FirstOrDefault(
                    u=>u.Etiqueta_Id == idEtiqueta && u.Articulo_Id == idArticulo
                );
                _Context.ArticuloEtiqueta.Remove(articuloEtiqueta);
                _Context.SaveChanges();
            
            return RedirectToAction(nameof(AdministrarEtiquetas), new
            {
                @id = idArticulo
            });
        }
    }

}
