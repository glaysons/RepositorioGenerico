using System;
using System.Data;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Search
{

	public class ConfiguradorCondicao<TObjeto> : IConfiguracaoCondicao<TObjeto>
	{

		private readonly ConfiguradorQuery<TObjeto> _configurador;
		private readonly string _campo;
		private readonly IQueryBuilder _queryBuilder;

		public ConfiguradorCondicao(ConfiguradorQuery<TObjeto> configurador, string campo, IQueryBuilder queryBuilder)
		{
			_configurador = configurador;
			_campo = campo;
			_queryBuilder = queryBuilder;
		}

		public IConfiguracaoQuery<TObjeto> Igual(string valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.String, valor);
		}

		private IConfiguracaoQuery<TObjeto> AdicionarCondicaoEConfiguracaoQuery(object operador, DbType tipo, object valor)
		{
			var parametro = _queryBuilder.AdicionarCondicao(_campo, (int)operador, valor);
			if (!string.IsNullOrEmpty(parametro))
				_configurador.DefinirParametro(parametro).Tipo(tipo, valor);
			return _configurador;
		}

		public IConfiguracaoQuery<TObjeto> Igual(int valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Int32, valor);
		}

		public IConfiguracaoQuery<TObjeto> Igual(int? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Int32, valor);
		}

		public IConfiguracaoQuery<TObjeto> Igual(bool valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Boolean, valor);
		}

		public IConfiguracaoQuery<TObjeto> Igual(double valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Double, valor);
		}

		public IConfiguracaoQuery<TObjeto> Igual(double? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Double, valor);
		}

		public IConfiguracaoQuery<TObjeto> Igual(decimal valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery<TObjeto> Igual(decimal? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery<TObjeto> Igual(DateTime valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.DateTime, valor);
		}

		public IConfiguracaoQuery<TObjeto> Igual(DateTime? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(Operadores.Igual, DbType.DateTime, valor);
		}

		public IConfiguracaoQuery<TObjeto> InicieCom(string valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresTexto.Contendo, DbType.String, valor + "%");
		}

		public IConfiguracaoQuery<TObjeto> TermineCom(string valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresTexto.NaoContendo, DbType.String, "%" + valor);
		}

		public IConfiguracaoQuery<TObjeto> Contenha(string valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresTexto.Contendo, DbType.String, "%" + valor + "%");
		}

		public IConfiguracaoQuery<TObjeto> NaoContenha(string valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresTexto.NaoContendo, DbType.String, "%" + valor + "%");
		}

		public IConfiguracaoQuery<TObjeto> Seja(Operadores operador, int valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Int32, valor);
		}

		public IConfiguracaoQuery<TObjeto> Seja(Operadores operador, int? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Int32, valor);
		}

		public IConfiguracaoQuery<TObjeto> Seja(Operadores operador, double valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Double, valor);
		}

		public IConfiguracaoQuery<TObjeto> Seja(Operadores operador, double? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Double, valor);
		}

		public IConfiguracaoQuery<TObjeto> Seja(Operadores operador, decimal valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery<TObjeto> Seja(Operadores operador, decimal? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery<TObjeto> Seja(Operadores operador, DateTime valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.DateTime, valor);
		}

		public IConfiguracaoQuery<TObjeto> Seja(Operadores operador, DateTime? valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, DbType.DateTime, valor);
		}

		public IConfiguracaoQuery<TObjeto> Seja(Operadores operador, DbType tipo, object valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(operador, tipo, valor);
		}

		public IConfiguracaoQuery<TObjeto> Entre(int inicio, int fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Int32);
		}

		private IConfiguracaoQuery<TObjeto> AdicionarCondicaoSqlBetween(object inicio, object fim, DbType tipo)
		{
			var parametros = _queryBuilder.AdicionarCondicaoEntre(_campo, inicio, fim);
			if (!string.IsNullOrEmpty(parametros[0]))
				_configurador.DefinirParametro(parametros[0]).Tipo(tipo, inicio);
			if (!string.IsNullOrEmpty(parametros[1]))
				_configurador.DefinirParametro(parametros[1]).Tipo(tipo, fim);
			return _configurador;
		}

		public IConfiguracaoQuery<TObjeto> Entre(int? inicio, int? fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Int32);
		}

		public IConfiguracaoQuery<TObjeto> Entre(double inicio, double fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Double);
		}

		public IConfiguracaoQuery<TObjeto> Entre(double? inicio, double? fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Double);
		}

		public IConfiguracaoQuery<TObjeto> Entre(decimal inicio, decimal fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Decimal);
		}

		public IConfiguracaoQuery<TObjeto> Entre(decimal? inicio, decimal? fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.Decimal);
		}

		public IConfiguracaoQuery<TObjeto> Entre(DateTime inicio, DateTime fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.DateTime);
		}

		public IConfiguracaoQuery<TObjeto> Entre(DateTime? inicio, DateTime? fim)
		{
			return AdicionarCondicaoSqlBetween(inicio, fim, DbType.DateTime);
		}

		public IConfiguracaoQuery<TObjeto> EstejaDentroDaLista(params string[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.In, DbType.String, valor);
		}

		public IConfiguracaoQuery<TObjeto> EstejaDentroDaLista(params int[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.In, DbType.Int32, valor);
		}

		public IConfiguracaoQuery<TObjeto> EstejaDentroDaLista(params double[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.In, DbType.Double, valor);
		}

		public IConfiguracaoQuery<TObjeto> EstejaDentroDaLista(params decimal[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.In, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery<TObjeto> EstejaForaDaLista(params string[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.NotIn, DbType.String, valor);
		}

		public IConfiguracaoQuery<TObjeto> EstejaForaDaLista(params int[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.NotIn, DbType.Int32, valor);
		}

		public IConfiguracaoQuery<TObjeto> EstejaForaDaLista(params double[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.NotIn, DbType.Double, valor);
		}

		public IConfiguracaoQuery<TObjeto> EstejaForaDaLista(params decimal[] valor)
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.NotIn, DbType.Decimal, valor);
		}

		public IConfiguracaoQuery<TObjeto> EhNulo()
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.Is, DbType.String, null);
		}

		public IConfiguracaoQuery<TObjeto> NaoEhNulo()
		{
			return AdicionarCondicaoEConfiguracaoQuery(OperadoresEspeciais.IsNot, DbType.String, null);
		}

	}
}
