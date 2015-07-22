using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Reflection;
using MultiSafepayTest.Business.Facade;

namespace MultiSafepayTest.Business.Facade
{
    public class MSPData : DbContext
    {
        ContextConfiguration configuration;

        public MSPData()
            : base("name=msp")
        {
            this.configuration = new ContextConfiguration();
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            foreach (var map in configuration.Configurations)
            {
                modelBuilder.Configurations.Add((dynamic)map);
            }
            
            foreach (var dbset in configuration.DbSets)
            {
                MethodInfo method = modelBuilder.GetType().GetMethod("Entity");
                method = method.MakeGenericMethod(new Type[] { dbset });
                method.Invoke(modelBuilder, null);
            }
            
            base.OnModelCreating(modelBuilder);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        
       
    }
}
