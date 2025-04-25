using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student_MVC.Models
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> optionss)
            : base(optionss)
        {
        }
        public DbSet<AdminModel> Admins { get; set; }

        public DbSet<StudentsModel> Students { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilding)
        {
            base.OnModelCreating(modelBuilding);


        }

    }
}
