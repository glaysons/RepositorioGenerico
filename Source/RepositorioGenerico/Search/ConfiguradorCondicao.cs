using System;
using System.Data;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search
{
	public class ConfiguradorCondicao: IConfiguracaoCondicao
	{
		private readonly ConfiguradorQuery _configurador;
		private readonly string _campo;
		private readonly IQueryBuilder _queryBuilder;

		public ConfiguradorCondicao(ConfiguradorQuery configurador, string campo, IQueryBuilder queryBuilder)
		{
			_configurador = configurador;
			_campo = campo;
			_queryBuilder = queryBuilder;
		}

		public IConfiguracaoQuery Igual(string valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.String, valor);
		}

		private IConfiguracaoQuery AdicionarCondicaoEConfiguracaoQuery(object operador, DbType tipo, object valor)
		{
			var parametro = _queryBuilder.AdicionarCondicao(_campo, (int)operador, valor);
			if (!string.IsNullOrEmpty(parametro))
				_configurador.DefinirParametro(parametro).Tipo(tipo, valor);
			return _configurador;
		}

		public IConfiguracaoQuery Igual(int valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Int32, valor);
		}

		public IConfiguracaoQuery Igual(int? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Int32, valor);
		}

		public IConfiguracaoQuery Igual(bool valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Boolean, valor);
		}

		public IConfiguracaoQuery Igual(double valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Double, valor);
		}

		public IConfiguracaoQuery Igual(double? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Double, valor);
		}

		public IConfiguracaoQuery Igual(decimal valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery Igual(decimal? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery Igual(DateTime valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.DateTime, valor);
		}

		public IConfiguracaoQuery Igual(DateTime? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.DateTime, valor);
		}

		public IConfiguracaoQuery InicieCom(string valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresTexto.Contendo, DbType.String, valor + "%");
		}

		public IConfiguracaoQuery TermineCom(string valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresTexto.NaoContendo, DbType.String, "%" + valor);
		}

		public IConfiguracaoQuery Contenha(string valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresTexto.Contendo, DbType.String, "%" + valor + "%");
		}

		public IConfiguracaoQuery NaoContenha(string valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresTexto.NaoContendo, DbType.String, "%" + valor + "%");
		}

		public IConfiguracaoQuery Seja(Operadores operador, int valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Int32, valor);
		}

		public IConfiguracaoQuery Seja(Operadores operador, int? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Int32, valor);
		}

		public IConfiguracaoQuery Seja(Operadores operador, double valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Double, valor);
		}

		public IConfiguracaoQuery Seja(Operadores operador, double? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Double, valor);
		}

		public IConfiguracaoQuery Seja(Operadores operador, decimal valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery Seja(Operadores operador, decimal? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery Seja(Operadores operador, DateTime valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.DateTime, valor);
		}

		public IConfiguracaoQuery Seja(Operadores operador, DateTime? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.DateTime, valor);
		}

		public IConfiguracaoQuery Seja(Operadores operador, DbType tipo, object valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, tipo, valor);
		}

		public IConfiguracaoQuery Entre(int inicio, int fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Int32);
		}

		private IConfiguracaoQuery AdicionarCondicaoSqlBetween(object inicio, object fim, DbType tipo)
		{
			var parametros = _queryBuilder.AdicionarCondicaoEntre(_campo, inicio, fim);
			if (!string.IsNullOrEmpty(parametros[0]))
				_configurador.DefinirParametro(parametros[0]).Tipo(tipo, inicio);
			if (!string.IsNullOrEmpty(parametros[1]))
				_configurador.DefinirParametro(parametros[1]).Tipo(tipo, fim);
			return _configurador;
		}

		public IConfiguracaoQuery Entre(int? inicio, int? fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Int32);
		}

		public IConfiguracaoQuery Entre(double inicio, double fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Double);
		}

		public IConfiguracaoQuery Entre(double? inicio, double? fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Double);
		}

		public IConfiguracaoQuery Entre(decimal inicio, decimal fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Decimal);
		}

		public IConfiguracaoQuery Entre(decimal? inicio, decimal? fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Decimal);
		}

		public IConfiguracaoQuery Entre(DateTime inicio, DateTime fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.DateTime);
		}

		public IConfiguracaoQuery Entre(DateTime? inicio, DateTime? fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.DateTime);
		}

		public IConfiguracaoQuery EstejaDentroDaLista(params string[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.In, DbType.String, valor);
		}

		public IConfiguracaoQuery EstejaDentroDaLista(params int[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.In, DbType.Int32, valor);
		}

		public IConfiguracaoQuery EstejaDentroDaLista(params double[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.In, DbType.Double, valor);
		}

		public IConfiguracaoQuery EstejaDentroDaLista(params decimal[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.In, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery EstejaForaDaLista(params string[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.NotIn, DbType.String, valor);
		}

		public IConfiguracaoQuery EstejaForaDaLista(params int[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.NotIn, DbType.Int32, valor);
		}

		public IConfiguracaoQuery EstejaForaDaLista(params double[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.NotIn, DbType.Double, valor);
		}

		public IConfiguracaoQuery EstejaForaDaLista(params decimal[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.NotIn, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery EhNulo()
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.Is, DbType.String, null);
		}

		public IConfiguracaoQuery NaoEhNulo()
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.IsNot, DbType.String, null);
		}

	}
}
