using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Lifestyle;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace MultiSafepayTest.Business.Facade
{
    public class FacadeFactory
    {
        private volatile static FacadeFactory factory;
        private IWindsorContainer container;
        
        private FacadeFactory()
        {
            container = new WindsorContainer();
            //we create a facade/db once per web request if there is a httpcontext
            //or transient if there is no httpcontext
            //transient means create when you call resolve
            container.Register(Classes.FromThisAssembly().BasedOn<BaseFacade>()
                .LifestyleScoped<HybridPerWebRequestTransientScopeAccessor>());
            container.Register(Classes.FromThisAssembly().BasedOn<DbContext>()
                //.LifestyleScoped<HybridPerWebRequestTransientScopeAccessor>());
                .LifestyleScoped<HybridPerWebRequestPerThreadScopeAccessor>());

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static FacadeFactory GetInstance()
        {
            if (factory == null)
                factory = new FacadeFactory();

            return factory;
        }

        public T GetFacade<T>() where T : BaseFacade
        {
            return container.Resolve<T>();
        }
    }
}
