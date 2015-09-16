using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using AzureHunter.Models;

namespace AzureHunter.Repository
{
    public class HunterDbContext:DbContext
    {
        // Constructors
        public HunterDbContext()
            : base("name=AzureHunterDatabaseCnn")
        {

        }

        public HunterDbContext(string cnnString)
            :base(cnnString)
        {
 
        }

        // Properties
        public DbSet<Item> Items { get; set; }
        public DbSet<File> Files { get; set; }

        public System.Data.Entity.DbSet<AzureHunter.ViewModels.ViewModelFile> ViewModelFiles { get; set; }
    }
}