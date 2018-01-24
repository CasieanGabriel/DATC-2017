using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerareDateApi.Models
{
    public class InregistrareContext : DbContext
    {
       
            public InregistrareContext(DbContextOptions<InregistrareContext> options)
                : base(options)
            {
            }

            public DbSet<Inregistrare> InregistrariItems { get; set; }

        
    }
}
