using Autofac;
using Autofac.Integration.Mvc;
using Entities;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Business.Mvc
{
	public class IocConfig
	{

		public static void RegistrarIoc()
		{
			var builder = new ContainerBuilder();

			RegistrarClassesDeNegocio(builder);

			RegistrarControllersDoMvc(builder);

			RegistrarConexaoPrincipal(builder);

			RegistrarTodosOsValueObjects(builder);

			InstalarIocNoMvc(builder);
		}

		private static void RegistrarClassesDeNegocio(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(typeof(ManutencaoCidadeBusiness).Assembly)
				.Where(t => t.Name.EndsWith("Business"))
				.InstancePerRequest();
		}

		private static void RegistrarControllersDoMvc(ContainerBuilder builder)
		{
			builder.RegisterControllers(typeof(IocConfig).Assembly)
				.InstancePerRequest();
		}

		private static void RegistrarConexaoPrincipal(ContainerBuilder builder)
		{
			var cs = ConfigurationManager.ConnectionStrings["ConexaoPadrao"].ConnectionString;
			RepositorioGenerico.SqlClient.Autofac.Configurador.RegisterSqlClient(builder, cs);
		}

		private static void RegistrarTodosOsValueObjects(ContainerBuilder builder)
		{
			RepositorioGenerico.SqlClient.Autofac.Configurador.RegisterModels(builder, typeof(Cidade).Assembly);
		}

		private static void InstalarIocNoMvc(ContainerBuilder builder)
		{
			builder.RegisterModule<AutofacWebTypesModule>();
			var container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}
	}
}