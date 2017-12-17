using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.SqlClient.Contextos;
using System.Data.SqlClient;

namespace RepositorioGenerico.SqlClient
{
	public static class Fabrica
	{

		public static IContexto CriarContexto(string stringConexao)
		{
			return new Contexto(stringConexao);
		}

		public static IContexto CriarContexto(string stringConexao, SqlTransaction transacao)
		{
			return new Contexto(stringConexao, transacao);
		}

		public static IContextoTransacional CriarContextoTransacional(string stringConexao)
		{
			return new Contexto(stringConexao);
		}

		public static Pattern.Contextos.Tables.IContexto CriarContextoLegado(string stringConexao)
		{
			return new Contextos.Tables.Contexto(stringConexao);
		}

		public static Pattern.Contextos.Tables.IContexto CriarContextoLegado(string stringConexao, SqlTransaction transacao)
		{
			return new Contextos.Tables.Contexto(stringConexao, transacao);
		}

		public static Pattern.Contextos.Tables.IContextoTransacional CriarContextoTransacionalLegado(string stringConexao)
		{
			return new Contextos.Tables.Contexto(stringConexao);
		}

	}
}
