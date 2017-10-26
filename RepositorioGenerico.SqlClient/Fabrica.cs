using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.SqlClient.Contextos;

namespace RepositorioGenerico.SqlClient
{
	public static class Fabrica
	{

		public static IContexto CriarContexto(string stringConexao)
		{
			return new Contexto(stringConexao);
		}

		public static Pattern.Contextos.Tables.IContexto CriarContextoLegado(string stringConexao)
		{
			return new Contextos.Tables.Contexto(stringConexao);
		}

	}
}
