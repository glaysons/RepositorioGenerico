﻿using System;
using System.Data;
using System.Globalization;
using RepositorioGenerico.Exceptions;
using System.Collections.Generic;

namespace RepositorioGenerico.Fake
{
	internal class DbCommandFake : IDbCommand
	{
		private const string PARAMETROSPROCEDURES = "RepositorioGenerico.Fake.Contextos.Procedure";

		private readonly DataSet _bancoDeDadosVirtual;

		private readonly IDataParameterCollection _parameters;

		public IDbConnection Connection { get; set; }

		public IDbTransaction Transaction { get; set; }

		public string CommandText { get; set; }

		public int CommandTimeout { get; set; }

		public CommandType CommandType { get; set; }

		public IDataParameterCollection Parameters
		{
			get { return _parameters; }
		}

		public bool VerificacaoDeExistencia { get; set; }

		public UpdateRowSource UpdatedRowSource { get; set; }

		public DbCommandFake(DataSet bancoDeDadosVirtual)
		{
			_parameters = new DataParametersCollectionFake();
			_bancoDeDadosVirtual = bancoDeDadosVirtual;
		}

		public void Prepare()
		{
		}

		public void Cancel()
		{
		}

		public IDbDataParameter CreateParameter()
		{
			return new DbDataParameterFake();
		}

		public int ExecuteNonQuery()
		{
			if (!_bancoDeDadosVirtual.Tables.Contains(PARAMETROSPROCEDURES))
				throw new NaoFoiPossivelOResultadoDaProcedureFakeException(CommandText);
			var view = ConsultarResultadoScalarDaProcedure();
			if ((view.Count > 0) && (view[0]["RegistrosAfetados"] != DBNull.Value))
				return Convert.ToInt32(view[0]["RegistrosAfetados"]);
			return 0;
		}

		public IDataReader ExecuteReader()
		{
			if (CommandType == CommandType.StoredProcedure)
				return CriarProcedure();
			return CriarQuery();
		}

		private IDataReader CriarProcedure()
		{
			var nome = "__proc__" + CommandText;
			if (_bancoDeDadosVirtual.Tables.Contains(nome))
				return new DataReaderFake(_bancoDeDadosVirtual.Tables[nome].DefaultView, 0);
			if (_bancoDeDadosVirtual.Tables.Contains(PARAMETROSPROCEDURES))
				return new DataReaderFake(ConsultarResultadoScalarDaProcedure(), 0);
			throw new NaoFoiPossivelOResultadoDaProcedureFakeException(CommandText);
		}

		private DataView ConsultarResultadoScalarDaProcedure()
		{
			var view = _bancoDeDadosVirtual.Tables[PARAMETROSPROCEDURES].DefaultView;
			view.RowFilter = string.Concat("Nome = '", CommandText, "'");
			return view;
		}

		private IDataReader CriarQuery()
		{
			if (EhConsultaRelacionada())
				return CriarQueryRelacionado();
			return CriarQueryDireto();
		}

		private bool EhConsultaRelacionada()
		{
			return (CommandText.StartsWith("asc|"))
				|| (CommandText.StartsWith("desc|"));
		}

		private IDataReader CriarQueryRelacionado()
		{
			int top;
			var view = ConsultarDataViewDaConsultaAtual(out top);
			var relacionamento = new DadosRelacionamento(CommandText);
			var relacao = CriarOuConsultarRelacaoEntreAsTabelas(relacionamento, view.Table.PrimaryKey);
			using (var novaTabela = _bancoDeDadosVirtual.Tables[relacionamento.Tabela].Clone())
			{
				foreach (DataRowView registro in view)
				{
					var novaView = registro.CreateChildView(relacao);
					foreach (DataRowView novoRegistro in novaView)
						novaTabela.ImportRow(novoRegistro.Row);
				}
				return new DataReaderFake(novaTabela.DefaultView);
			}
		}

		private DataRelation CriarOuConsultarRelacaoEntreAsTabelas(DadosRelacionamento relacionamento, DataColumn[] chavesPrimarias)
		{
			var nome = relacionamento.Tabela + "_" + relacionamento.CamposEstrangeiros.Replace(",", "_").Replace(" ", string.Empty);
			if (_bancoDeDadosVirtual.Relations.Contains(nome))
				return _bancoDeDadosVirtual.Relations[nome];
			var relacao = new DataRelation(nome, chavesPrimarias, ConsultarCamposChaveRelacionadosComTabelaFilha(relacionamento));
			_bancoDeDadosVirtual.Relations.Add(relacao);
			return relacao;
		}

		private DataColumn[] ConsultarCamposChaveRelacionadosComTabelaFilha(DadosRelacionamento relacionamento)
		{
			var tabela = _bancoDeDadosVirtual.Tables[relacionamento.Tabela];
			var campos = new List<DataColumn>();
			foreach (var campo in relacionamento.CamposEstrangeiros.Split(','))
				campos.Add(tabela.Columns[campo]);
			return campos.ToArray();
		}

		private IDataReader CriarQueryDireto()
		{
			int top;
			var view = ConsultarDataViewDaConsultaAtual(out top);
			return new DataReaderFake(view, top, VerificacaoDeExistencia);
		}

		private DataView ConsultarDataViewDaConsultaAtual(out int top)
		{
			var view = ConsultarTabelaComDadosVirtuais().DefaultView;
			var sql = CommandText.ToLower();
			var indiceTop = sql.IndexOf("select top ", StringComparison.Ordinal);
			var fimTop = sql.IndexOf(" ", 11, StringComparison.Ordinal);
			top = ((indiceTop == 0) && (fimTop > 0))
				? Convert.ToInt32(sql.Substring(11, fimTop - 11))
				: 0;
			var indiceWhere = sql.IndexOf("where", StringComparison.Ordinal);
			var condicao = (indiceWhere > 0)
				? ConsultarCondicaoParaFiltragem(CommandText.Substring(indiceWhere + 5))
				: string.Empty;
			var indiceOrderby = (indiceWhere > 0 ? condicao : sql)
				.IndexOf("order by", StringComparison.Ordinal);
			if (indiceOrderby > 0)
			{
				if (indiceWhere > 0)
				{
					view.Sort = condicao.Substring(indiceOrderby + 8);
					condicao = condicao.Substring(0, indiceOrderby);
				}
				else
					view.Sort = CommandText.Substring(indiceOrderby + 8);
			}
			view.RowFilter = condicao;
			return view;
		}

		private DataTable ConsultarTabelaComDadosVirtuais()
		{
			VerificarSeEhTabelaDeValidacaoDeExistencia();
			if (_bancoDeDadosVirtual.Tables.Count == 1)
				return _bancoDeDadosVirtual.Tables[0];
			var sql = CommandText.ToLower();
			var from = sql.IndexOf("from", StringComparison.Ordinal);
			if (from == -1)
				throw new NaoFoiPossivelDeterminarONomeDaTabelaFakeException();
			sql = sql.Substring(from + 4).Trim();
			var corte = sql.IndexOf(sql.StartsWith("[") ? "]" : " ", StringComparison.Ordinal);
			if (corte == -1)
				throw new NaoFoiPossivelDeterminarONomeDaTabelaFakeException();
			var nome = sql.Substring(0, corte)
				.Replace("[", string.Empty)
				.Replace("]", string.Empty);
			if (!_bancoDeDadosVirtual.Tables.Contains(nome))
				throw new NaoFoiPossivelDeterminarONomeDaTabelaFakeException();
			return _bancoDeDadosVirtual.Tables[nome];
		}

		private void VerificarSeEhTabelaDeValidacaoDeExistencia()
		{
			VerificacaoDeExistencia = (CommandText?.StartsWith("select top 1 1 from") == true);
		}

		private string ConsultarCondicaoParaFiltragem(string condicao)
		{
			foreach (IDbDataParameter parametro in Parameters)
				condicao = condicao.Replace(parametro.ParameterName, ConverterParametroEmConstange(parametro));
			return condicao;
		}

		private string ConverterParametroEmConstange(IDataParameter parametro)
		{
			if ((parametro.Value == DBNull.Value) || (parametro.Value == null))
				return "NULL";

			switch (parametro.DbType)
			{
				case DbType.Boolean:
					return Convert.ToBoolean(parametro.Value)
						? "True"
						: "False";

				case DbType.Date:
				case DbType.DateTime:
				case DbType.DateTime2:
				case DbType.DateTimeOffset:
					return "'" + Convert.ToDateTime(parametro.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'";

				case DbType.Currency:
				case DbType.Decimal:
				case DbType.Double:

				case DbType.Int16:
				case DbType.Int32:
				case DbType.Int64:
				case DbType.Single:

				case DbType.UInt16:
				case DbType.UInt32:
				case DbType.UInt64:
				case DbType.VarNumeric:
					return Convert.ToDouble(parametro.Value).ToString(CultureInfo.InvariantCulture)
						.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");

				default:
					return "'" + parametro.Value.ToString().Replace("'", "''") + "'";

			}
		}

		public IDataReader ExecuteReader(CommandBehavior behavior)
		{
			return ExecuteReader();
		}

		public object ExecuteScalar()
		{
			using (var reader = ExecuteReader())
				if (reader.Read())
					return reader[0];
			return null;
		}

		public void Dispose()
		{

		}

	}
}
