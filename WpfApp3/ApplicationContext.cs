using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.models;

namespace WpfApp3
{
    public class ApplicationContext : DbContext
    {
        public DbSet<models.Teacher> Teachers { get; set; }
        public DbSet<Fraction> Fractions { get; set; }
        public DbSet<TeacherAudience> TeacherAudiences { get; set; }
        public DbSet<Subdivision> Subdivisions { get; set; }
        public DbSet<DayOfTheWeek> DaysOfTheWeek { get; set; }
        public DbSet<models.Group> Groups { get; set; }
        public DbSet<ScheduleOfClass> ScheduleOfClasses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Couple> Couples { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=127.0.0.1;Port=3306;Database=timetable;Uid=root;Pwd=root;");
        }
    }
}
