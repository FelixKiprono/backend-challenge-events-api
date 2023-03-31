using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using demoapp.Models;

    public class EventDBContenxt : DbContext
    {
        public EventDBContenxt (DbContextOptions<EventDBContenxt> options)
            : base(options)
        {
        }

        public DbSet<demoapp.Models.Event> Event { get; set; } = default!;

        public DbSet<demoapp.Models.Attending>? Attending { get; set; }
    }
