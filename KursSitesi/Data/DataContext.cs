using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace whoindex.Data
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrolment>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId});

            modelBuilder.Entity<Enrolment>()
                .HasOne(sc => sc.Students)
                .WithMany(s => s.Enrolments)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<Enrolment>()
                .HasOne(sc => sc.Courses)
                .WithMany(c => c.Enrolments)
                .HasForeignKey(sc => sc.CourseId);
        }

//  **Bu konfigürasyon, ara tablodaki StudentId ve CourseId
//  **kolonlarının birleşik birincil anahtar (composite primary key) 
//  **olduğunu ve uygun ilişki kurallarını tanımlar.

    }




}