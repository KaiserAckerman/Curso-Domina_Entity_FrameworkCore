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
            //modelBuilder.Entity<ArticuloEtiqueta>().HasKey(ae => new {ae.Etiqueta_Id,ae.Articulo_Id});

            ////Siembra de datos se hace en este metodo
            //var categoria2 = new Categoria() { Categoria_Id = 182, Nombre = "Categoria 2", FechaCreacion = new DateTime(2025, 11, 28), Activo = true};
            //var categoria5 = new Categoria() { Categoria_Id = 183, Nombre = "Categoria 5", FechaCreacion = new DateTime(2025, 11, 29), Activo = false };
            //modelBuilder.Entity<Categoria>().HasData(new Categoria[] { categoria5 });

            //Fluent API para Categoria
            modelBuilder.Entity<Categoria>().HasKey(c=>c.Categoria_Id);
            modelBuilder.Entity<Categoria>().Property(c => c.Nombre).IsRequired();
            modelBuilder.Entity<Categoria>().Property(c => c.FechaCreacion).HasColumnType("date");

            //Fluent API para Articulo
            modelBuilder.Entity<Articulo>().HasKey(c => c.Articulo_Id);
            modelBuilder.Entity<Articulo>().Property(c => c.TituloArticulo).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Articulo>().Property(c => c.Descripcion).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Articulo>().Property(c => c.Fecha).HasColumnType("date");

            //Fluent API nombre de tabla y nombre de columna
            modelBuilder.Entity<Articulo>().ToTable("Tbl_Articulo");
            modelBuilder.Entity<Articulo>().Property(c => c.TituloArticulo).HasColumnName("Titulo");

            //Fluent API para Usuario
            modelBuilder.Entity<Usuario>().HasKey(c => c.Id);
            modelBuilder.Entity<Usuario>().Ignore(u => u.Edad);


            //Fluent API para DetalleUsuario
            modelBuilder.Entity<DetalleUsuario>().HasKey(c => c.DetalleUsuario_Id);
            modelBuilder.Entity<DetalleUsuario>().Property(c => c.Cedula).IsRequired();

            //Fluent API para Etiqueta
            modelBuilder.Entity<Etiqueta>().HasKey(c => c.Etiqueta_Id);
            modelBuilder.Entity<Etiqueta>().Property(c => c.Fecha).HasColumnType("date");

            //Fluent API: relacion de uno a uno entre Usuario y DetalleUsuario
            modelBuilder.Entity<Usuario>()
                .HasOne(c => c.DetalleUsuario)
                .WithOne(c => c.Usuario).HasForeignKey<Usuario>("DetalleUsuario_Id");

            //Fluent API: relacion de uno a muchos entre Categoria y Articulo
            modelBuilder.Entity<Articulo>()
                .HasOne(c => c.Categoria)
                .WithMany(c => c.Articulo).HasForeignKey(c=>c.Categoria_Id);

            //Fluent API: relacion de muchos a muchos entre Articulo y Etiqueta
            modelBuilder.Entity<ArticuloEtiqueta>().HasKey
                (ae => new { ae.Etiqueta_Id, ae.Articulo_Id });
            modelBuilder.Entity<ArticuloEtiqueta>()
                .HasOne(a => a.Articulo)
                .WithMany(a => a.ArticuloEtiqueta).HasForeignKey(c => c.Articulo_Id);
            modelBuilder.Entity<ArticuloEtiqueta>()
                .HasOne(a => a.Etiqueta)
                .WithMany(a => a.ArticuloEtiqueta).HasForeignKey(c => c.Etiqueta_Id);

            base.OnModelCreating(modelBuilder);
        }
        //Cuando crear migraciones (Buenas practicas)
        //1-Se crea una nueva clase(tabla en la Bd)
        //2-Cuando agrege una nueva propiedad(Crear un campo nuevo en la Bd)
        //3=Cuando modifique un valor de campo en la clase(modificar un campo en Bd)
    }
}
