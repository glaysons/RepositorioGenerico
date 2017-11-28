using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using ConverterBancoParaEntidades.Interfaces;
using ConverterBancoParaEntidades.Estruturas;
using ConverterBancoParaEntidades.Constantes;

namespace ConverterBancoParaEntidades.Consultadores
{
	public class SqlClient : DataClient<SqlConnection>
	{

		private int[] tiposString = new[] { 35, 99, 167, 175, 231, 239, 241 };
		private int[] tiposInteiro = new[] { 48, 52, 56, 127 };
		private int[] tiposBoolean = new[] { 104 };
		private int[] tiposDecimal = new[] { 60, 106, 108, 122 };
		private int[] tiposDateTime = new[] { 58, 61 };
		private int[] tiposDouble = new[] { 59, 62 };
		private int[] tiposGuid = new[] { 36 };
		private int[] tiposImagem = new[] { 34 };

		public SqlClient(IConfiguracao configuracao) 
			: base(configuracao)
		{

		}

		public override IList<Campo> ConsultarCamposDaTabela(string tabela)
		{
			using (var conexao = new SqlConnection())
			{
				conexao.ConnectionString = Configuracao.Conexao;
				conexao.Open();
				var campos = CriarListaDeCampos(conexao, tabela);
				AtualizarCamposChave(conexao, tabela, campos);
				AtualizarCamposIdentity(conexao, tabela, campos);
				return campos;
			}
		}

		private IList<Campo> CriarListaDeCampos(SqlConnection conexao, string tabela)
		{
			var lista = new List<Campo>();

			using (var comando = conexao.CreateCommand())
			{
				comando.CommandText = string.Concat(
					"select c.*, (",
						"select t.name ",
						"from systypes t ",
						"where (t.xtype = c.xtype)) as xtypename ",
					"from syscolumns c ",
					"where (c.id = object_id('", tabela, "'))");
				comando.CommandTimeout = 0;

				var reader = comando.ExecuteReader();

				while (reader.Read())
					AdicionarNovoCampoNaLista(lista, reader);
			}

			return lista;
		}

		private void AdicionarNovoCampoNaLista(List<Campo> lista, SqlDataReader reader)
		{
			var campo = new Campo();
			campo.Ordem = Convert.ToInt32(reader["colorder"]);
			campo.NomeCampo = reader["name"].ToString();
			campo.TipoBanco = Convert.ToInt32(reader["xtype"]);
			campo.TipoInterno = ConsultarTipoCampo(campo.TipoBanco);
			campo.NomeTipo = reader["xtypename"].ToString();
			campo.Obrigatorio = (Convert.ToInt32(reader["isnullable"]) == 0);
			campo.Chave = false;
			campo.Identity = false;
			campo.TamanhoMaximo = (campo.TipoInterno == TipoCampo.String)
				? Convert.ToInt32(reader["length"])
				: 0;
			campo.Calculado = (Convert.ToInt32(reader["iscomputed"]) == 1);
			lista.Add(campo);
		}

		private void AtualizarCamposChave(SqlConnection conexao, string tabela, IList<Campo> campos)
		{
			using (var comando = conexao.CreateCommand())
			{
				comando.CommandText = "exec sp_pkeys [" + tabela + "]";
				comando.CommandTimeout = 0;
				var reader = comando.ExecuteReader();
				while (reader.Read())
				{
					var nomeCampo = reader["COLUMN_NAME"].ToString();
					foreach (var campo in campos.Where(c => string.Equals(c.NomeCampo, nomeCampo, StringComparison.InvariantCultureIgnoreCase)))
						campo.Chave = true;
				}
			}
		}

		private void AtualizarCamposIdentity(SqlConnection conexao, string tabela, IList<Campo> campos)
		{
			using (var comando = conexao.CreateCommand())
			{
				comando.CommandText = "select name from sys.identity_columns i where (i.object_id = object_id('" + tabela + "'))";
				comando.CommandTimeout = 0;
				var reader = comando.ExecuteReader();
				while (reader.Read())
				{
					var nomeCampo = reader.GetString(0);
					foreach (var campo in campos.Where(c => string.Equals(c.NomeCampo, nomeCampo, StringComparison.InvariantCultureIgnoreCase)))
						campo.Identity = true;
				}
			}
		}

		private TipoCampo ConsultarTipoCampo(int tipo)
		{
			if (tiposString.Contains(tipo))
				return TipoCampo.String;

			if (tiposInteiro.Contains(tipo))
				return TipoCampo.Inteiro;

			if (tiposBoolean.Contains(tipo))
				return TipoCampo.Boolean;

			if (tiposDecimal.Contains(tipo))
				return TipoCampo.Decimal;

			if (tiposDateTime.Contains(tipo))
				return TipoCampo.DateTime;

			if (tiposDouble.Contains(tipo))
				return TipoCampo.Double;

			if (tiposGuid.Contains(tipo))
				return TipoCampo.Guid;

			if (tiposImagem.Contains(tipo))
				return TipoCampo.Imagem;

			return TipoCampo.Object;
		}

		public override IEnumerable<Relacionamento> ConsultarRelacionamentosAscendentesDaTabela(string tabela)
		{
			using (var conexao = new SqlConnection())
			{
				conexao.ConnectionString = Configuracao.Conexao;
				conexao.Open();
				var lista = new List<Relacionamento>();

				using (var comando = conexao.CreateCommand())
				{
					comando.CommandText = string.Concat("exec sp_foreign_keys_rowset2 @foreignkey_tab_name = '", tabela, "'");
					comando.CommandTimeout = 0;

					var reader = comando.ExecuteReader();

					while (reader.Read())
						AdicionarNovoRelacionamentoAscendenteNaLista(lista, reader);
				}

				return lista;
			}
		}

		private void AdicionarNovoRelacionamentoAscendenteNaLista(List<Relacionamento> lista, SqlDataReader reader)
		{
			var relacionamento = new Relacionamento();
			relacionamento.Ordem = Convert.ToInt32(reader["ORDINAL"]);
			relacionamento.Nome = reader["FK_NAME"].ToString();
			relacionamento.TabelaChavePrimaria = reader["PK_TABLE_NAME"].ToString();
			relacionamento.ColunaChavePrimaria = reader["PK_COLUMN_NAME"].ToString();
			relacionamento.ColunaChaveEstrangeira = reader["FK_COLUMN_NAME"].ToString();
			lista.Add(relacionamento);
		}

		public override IEnumerable<Relacionamento> ConsultarRelacionamentosDescendentesDaTabela(string tabela)
		{
			using (var conexao = new SqlConnection())
			{
				conexao.ConnectionString = Configuracao.Conexao;
				conexao.Open();
				var lista = new List<Relacionamento>();

				using (var comando = conexao.CreateCommand())
				{
					comando.CommandText = string.Concat("exec sp_fkeys '", tabela, "'");
					comando.CommandTimeout = 0;

					var reader = comando.ExecuteReader();

					while (reader.Read())
						AdicionarNovoRelacionamentoDescendenteNaLista(lista, reader);
				}

				return lista;
			}
		}

		private void AdicionarNovoRelacionamentoDescendenteNaLista(List<Relacionamento> lista, SqlDataReader reader)
		{
			var relacionamento = new Relacionamento();
			relacionamento.Ordem = Convert.ToInt32(reader["KEY_SEQ"]);
			relacionamento.Nome = reader["FK_NAME"].ToString();
			relacionamento.TabelaChavePrimaria = reader["PKTABLE_NAME"].ToString();
			relacionamento.TabelaChaveEstrangeira = reader["FKTABLE_NAME"].ToString();
			relacionamento.ColunaChaveEstrangeira = reader["FKCOLUMN_NAME"].ToString();
			lista.Add(relacionamento);
		}

	}
}
