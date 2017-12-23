using System.Configuration;

namespace RepositorioGenerico.Test
{
	internal static class ConnectionStringHelper
	{

		public static string Consultar()
		{
			return ConfigurationManager.ConnectionStrings["ConexaoPadrao"].ConnectionString;
		}

	}
}
