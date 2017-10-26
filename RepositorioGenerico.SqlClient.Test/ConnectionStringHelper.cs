using System.Configuration;

namespace RepositorioGenerico.SqlClient.Test
{
	public static class ConnectionStringHelper
	{

		public static string Consultar()
		{
			return ConfigurationManager.ConnectionStrings["ConexaoPadrao"].ConnectionString;
		}

	}
}
