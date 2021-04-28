using ADataCenter.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADataCenter.Data
{
    public class IncidentContext : DbContext
    {
       

        public IncidentContext(DbContextOptions<IncidentContext> options):base(options)
        {
            
        }
        DbSet<Incident> _Incidents = null;
        DbSet<Incident_Handling> _IncidentHandling = null;

        public DbSet<Incident> Incidents
        {
            get
            {
                return _Incidents;
            }
            set
            {
                _Incidents = value;
            }
        }

        public DbSet<Incident_Handling> incidentHandling
        {
            get
            {
                return _IncidentHandling;
            }
            set
            {
                _IncidentHandling = value;
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=incidentdb;Username=postgres;Password=postgres", o=> o.UseNodaTime).;
            //string cs = string.Empty;
            //optionsBuilder.UseSqlServer(cs);
            //optionsBuilder.UseInMemoryDatabase("IncidentDb");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<incident_handling>()
            //    .HasNoKey();
        }
    }
}
