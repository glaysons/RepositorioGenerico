using ConverterBancoParaEntidades.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace ConverterBancoParaEntidades.Consultadores
{
	public class DataClient<TDriver> : IConsultador where TDriver : IDbConnection, new()
	{

		private IConfiguracao _configuracao;

		public DataClient(IConfiguracao configuracao)
		{
			_configuracao = configuracao;
		}

		public string[] Consultar()
		{
			var itens = new List<string>();

			using (var conexao = new TDriver())
			{
				conexao.ConnectionString = _configuracao.Conexao;
				using (var comando = conexao.CreateCommand())
				{
					comando.CommandText = _configuracao.ScriptConsultaTabelas;
					comando.CommandTimeout = 0;
					conexao.Open();
					var reader = comando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
					while (reader.Read())
						itens.Add(reader.GetString(0));
				}
			}

			return itens.ToArray();
		}

	}
}
