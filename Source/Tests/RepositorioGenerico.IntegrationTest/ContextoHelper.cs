using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.SqlClient;
using System.Configuration;

namespace RepositorioGenerico.IntegrationTest
{
	public static class ContextoHelper
	{

		public static IContexto Criar()
		{
			var cs = ConfigurationManager.ConnectionStrings["ConexaoPadrao"].ConnectionString;
			return Fabrica.CriarContexto(cs);
		}

	}
}
