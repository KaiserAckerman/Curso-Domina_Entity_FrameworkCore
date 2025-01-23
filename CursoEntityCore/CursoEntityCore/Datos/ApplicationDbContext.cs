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

        //Cuando crear migraciones (Buenas practicas)
        //1-Se crea una nueva clase(tabla en la Bd)
        //2-Cuando agrege una nueva propiedad(Crear un campo nuevo en la Bd)
        //3=Cuando modifique un valor de campo en la clase(modificar un campo en Bd)
    }
}
