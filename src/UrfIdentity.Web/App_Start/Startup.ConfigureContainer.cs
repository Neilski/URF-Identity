using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Identity;
using Microsoft.AspNet.Identity;
using Owin;
using UrfIdentity.DAL.Db;
using UrfIdentity.DAL.Repository.Infrastructure;
using UrfIdentity.DAL.Service;
using UrfIdentity.DAL.Service.Interfaces;
using UrfIdentity.Models;


namespace UrfIdentity.Web
{
    /// <summary>
    /// Register types into the Autofac Inversion of Control (IOC) container. Autofac makes it easy to register common 
    /// MVC types like the <see cref="UrlHelper"/> using the <see cref="AutofacWebTypesModule"/>. Feel free to change 
    /// this to another IoC container of your choice but ensure that common MVC types like <see cref="UrlHelper"/> are 
    /// registered. See http://autofac.readthedocs.org/en/latest/integration/aspnet.html.
    /// </summary>
    public partial class Startup
    {
        public static void ConfigureContainer(IAppBuilder app)
        {
            IContainer container = CreateContainer();
            app.UseAutofacMiddleware(container);

            // Register MVC Types 
            app.UseAutofacMvc();
        }


        private static IContainer CreateContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            Assembly assembly = Assembly.GetExecutingAssembly();

            builder.RegisterApiControllers(assembly);

            RegisterServices(builder);
            RegisterDALs(builder);
            RegisterMvc(builder, assembly);

            IContainer container = builder.Build();

            SetWebApiDependencyResolver(container);
            SetMvcDependencyResolver(container);

            return container;
        }


        /// <summary>
        /// Sets the ASP.NET WebApi dependency resolver.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void SetWebApiDependencyResolver(IContainer container)
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        /// <summary>
        /// Sets the ASP.NET MVC dependency resolver.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void SetMvcDependencyResolver(IContainer container)
        {
            DependencyResolver.SetResolver(
                new AutofacDependencyResolver(container));
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            // Provide injection support for HttpContext
            // See: http://stackoverflow.com/a/15639347
            builder.RegisterModule(new AutofacWebTypesModule());
        }


        private static void RegisterMvc(
            ContainerBuilder builder,
            Assembly assembly)
        {
            // Register Common MVC Types
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register MVC Filters
            builder.RegisterFilterProvider();

            // Register MVC Controllers
            builder.RegisterControllers(assembly);
        }


        private static void RegisterDALs(ContainerBuilder builder)
        {
            // Data Context
            builder.RegisterType<UrfIdentityDataContext>()
                .As<IUrfIdentityDataContextAsync>()
                .InstancePerRequest();

            // Unit of Work
            builder.RegisterType<UrfIdentityUnitOfWork>()
                .As<IUrfIdentityUnitOfWorkAsync>()
                .InstancePerRequest();


            RegisterIdentity(builder);
            RegisterApplicationData(builder);
        }

        private static void RegisterIdentity(ContainerBuilder builder)
        {
            // Wire up the IdentityRoleManager
            builder.RegisterType<ApplicationRoleStore>()
                .As<IRoleStore<ApplicationRole, Guid>>()
                .InstancePerRequest();

            builder.RegisterType<ApplicationRoleManager>()
                .InstancePerRequest();


            // Repositories
            builder.RegisterType<UrfIdentityRepository<ApplicationUser>>()
                .As<IUrfIdentityRepositoryAsync<ApplicationUser>>()
                .InstancePerRequest();

            builder.RegisterType<UrfIdentityRepository<ApplicationRole>>()
                .As<IUrfIdentityRepositoryAsync<ApplicationRole>>()
                .InstancePerRequest();

            // Services
            builder.RegisterType<ApplicationUserService>()
                .As<IApplicationUserService>()
                .InstancePerRequest();

            builder.RegisterType<ApplicationRoleService>()
                .As<IApplicationRoleService>()
                .InstancePerRequest();
        }


        private static void RegisterApplicationData(ContainerBuilder builder)
        {
            // Add additional repository and service registrations here
        }
    }
}