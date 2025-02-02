using Microsoft.EntityFrameworkCore;
using projects_entity.Models;

namespace projects_entity
{
    public class ProjectsContext : DbContext
    {
        public ProjectsContext(DbContextOptions<ProjectsContext> options) : base(options) { }

        /// <summary>
        /// Комплекты документации
        /// </summary>
        public DbSet<Documentation> Documentations { get; set; }
        /// <summary>
        /// Справочник марок
        /// </summary>
        public DbSet<Mark> Marks { get; set; }
        /// <summary>
        /// Объекты проектирования
        /// </summary>
        public DbSet<Models.DesignObject> Objects { get; set; }
        /// <summary>
        /// Проекты
        /// </summary>
        public DbSet<Project> Projects { get; set; }

        /// <summary>
        /// Логи
        /// </summary>
        public DbSet<NLog> NLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DesignObject>()
                .HasOne(d => d.ParentObject)
                .WithMany(d => d.ChildObjects);
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Objects)
                .WithOne(o => o.Project);
            modelBuilder.Entity<Documentation>()
                .HasOne(d => d.Object)
                .WithOne(o => o.Documentation);
        }
    }
}
