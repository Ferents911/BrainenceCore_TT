using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainenceCore_Task.Models
{
   
    
    public class ApplicationContext : DbContext
    {
         public DbSet<Sentences> Text { get; set; }
         public ApplicationContext(DbContextOptions<ApplicationContext> options)
             : base(options)
         {
         }
    }
    
}
