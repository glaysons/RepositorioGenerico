using Autofac;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern;
using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.SqlClient.Contextos;
using System.Linq;
using System.Reflection;

namespace RepositorioGenerico.SqlClient.Autofac
{
	public static class Configurador
	{

		public static void RegisterSqlClient(ContainerBuilder builder, string stringConexao)
		{

			builder.RegisterType<Contexto>()
				.WithParameter("stringConexao", stringConexao)
				.As<IContexto>()
				.As<IContextoTransacional>()
				.As<IConexao>()
				.As<IEventoConexao>()
				.InstancePerLifetimeScope();

		}

		public static void RegisterModels(ContainerBuilder builder, Assembly assembly)
		{
			var tipoEntidade = typeof(Entidade);
			var tipoDicionario = typeof(Dicionario);
			var tipoPersistencia = typeof(Persistencia<>);
			var tipoRepositorio = typeof(Repositorio<>);
			var tipoIRepositorio = typeof(IRepositorio<>);

			foreach (var tipo in (assembly.GetTypes().Where(tipo => tipoEntidade.IsAssignableFrom(tipo))))
			{
				builder.RegisterType(tipoDicionario)
					.WithParameter("tipo", tipo)
					.SingleInstance();

				builder.RegisterType(tipoPersistencia.MakeGenericType(tipo))
					.InstancePerRequest();

				builder.RegisterType(tipoRepositorio.MakeGenericType(tipo))
					.As(tipoIRepositorio.MakeGenericType(tipo))
					.InstancePerRequest();
			}
		}

	}
}
