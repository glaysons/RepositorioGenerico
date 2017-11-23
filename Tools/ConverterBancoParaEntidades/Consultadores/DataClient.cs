using ConverterBancoParaEntidades.Constantes;
using ConverterBancoParaEntidades.Estruturas;
using ConverterBancoParaEntidades.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace ConverterBancoParaEntidades.Consultadores
{
	public abstract class DataClient<TDriver> : IConsultador where TDriver : IDbConnection, new()
	{

		private IConfiguracao _configuracao;

		protected IConfiguracao Configuracao
		{
			get { return _configuracao; }
		}

		public DataClient(IConfiguracao configuracao)
		{
			_configuracao = configuracao;
		}

		public string[] ConsultarTabelas()
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
					var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
					while (reader.Read())
						itens.Add(reader.GetString(0));
				}
			}

			return itens.ToArray();
		}

		public abstract IEnumerable<Campo> ConsultarCamposDaTabela(string tabela);

	}
}
