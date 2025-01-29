using CursoEntityCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CursoEntityCore.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones) : base(opciones)
        {
                
        }

        //Escribir los modelos
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<DetalleUsuario> DetalleUsuario { get; set; }
        public DbSet<Etiqueta> Etiqueta { get; set; }
        //Agregamos dbset para la tabla de relacion ArticuloEtiqueta
        public DbSet<ArticuloEtiqueta> ArticuloEtiqueta { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticuloEtiqueta>().HasKey(ae => new {ae.Etiqueta_Id,ae.Articulo_Id});

            //Siembra de datos se hace en este metodo
            var categoria2 = new Categoria() { Categoria_Id = 182, Nombre = "Categoria 2", FechaCreacion = new DateTime(2025, 11, 28), Activo = true};
            var categoria5 = new Categoria() { Categoria_Id = 183, Nombre = "Categoria 5", FechaCreacion = new DateTime(2025, 11, 29), Activo = false };

            modelBuilder.Entity<Categoria>().HasData(new Categoria[] { categoria5 });
            base.OnModelCreating(modelBuilder);
        }
        //Cuando crear migraciones (Buenas practicas)
        //1-Se crea una nueva clase(tabla en la Bd)
        //2-Cuando agrege una nueva propiedad(Crear un campo nuevo en la Bd)
        //3=Cuando modifique un valor de campo en la clase(modificar un campo en Bd)
    }
}
