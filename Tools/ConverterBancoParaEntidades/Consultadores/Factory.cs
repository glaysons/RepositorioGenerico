using System;
using ConverterBancoParaEntidades.Interfaces;
using System.Data.SqlClient;

namespace ConverterBancoParaEntidades.Consultadores
{
	public static class Factory
	{

		public static IConsultador CriarConsultador(IConfiguracao configuracao)
		{
			Validar(configuracao);
			return new DataClient<SqlConnection>(configuracao);
		}

		private static void Validar(IConfiguracao configuracao)
		{
			if (string.IsNullOrEmpty(configuracao.Conexao))
				throw new Exception("Favor informar uma conexão válida para um servidor de banco de dados!");
			if (string.IsNullOrEmpty(configuracao.ScriptConsultaTabelas))
				throw new Exception("Favor informar um script válido para consulta das tabelas do banco de dados!");
		}
	}
}
