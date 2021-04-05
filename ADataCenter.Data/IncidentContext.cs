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
        public IncidentContext()
        {

        }
        DbSet<Incident> _Incidents = null;
        
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string cs = string.Empty;
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=incidentdb;Username=postgres;Password=postgres");
            //optionsBuilder.UseSqlServer(cs);
            //optionsBuilder.UseInMemoryDatabase("IncidentDb");
        }

    }
}
