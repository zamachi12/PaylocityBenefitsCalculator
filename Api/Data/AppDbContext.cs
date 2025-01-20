using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using  Api.Models;
using Microsoft.EntityFrameworkCore.Design;
using Api.Dtos.Dependent;

namespace Api.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
        

        public DbSet<Employee> Employees {get; set ;}

        public DbSet<Dependent> Dependents {get; set ;}

    }

}