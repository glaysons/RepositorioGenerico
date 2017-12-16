using RepositorioGenerico.Pattern.Contextos;
using RepositorioGenerico.SqlClient.Contextos;
using System;
using System.Data.SqlClient;

namespace RepositorioGenerico.SqlClient
{
	public static class Fabrica
	{

		public static IContexto CriarContexto(string stringConexao)
		{
			return new Contexto(stringConexao);
		}

		public static IContexto CriarContexto(string stringConexao, SqlConnection conexao)
		{
			throw new NotImplementedException();
			//return new Contexto(stringConexao, conexao);
		}

		public static IContexto CriarContexto(string stringConexao, SqlTransaction transacao)
		{
			throw new NotImplementedException();
			//return new Contexto(stringConexao, transacao);
		}

		public static Pattern.Contextos.Tables.IContexto CriarContextoLegado(string stringConexao)
		{
			return new Contextos.Tables.Contexto(stringConexao);
		}

	}
}
