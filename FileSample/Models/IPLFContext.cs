using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FileSample.Models
{
    public class IPLFContext : DbContext
    {
        public IPLFContext() : base("IPLFContext")
        {
        }
        public DbSet<VehicleFile> VehicleFiles { get; set; }
    }
}