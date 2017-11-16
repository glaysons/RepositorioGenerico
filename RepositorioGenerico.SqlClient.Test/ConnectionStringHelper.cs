using System.Configuration;

namespace RepositorioGenerico.SqlClient.Test
{
	internal static class ConnectionStringHelper
	{

		public static string Consultar()
		{
			return ConfigurationManager.ConnectionStrings["ConexaoPadrao"].ConnectionString;
		}

	}
}
