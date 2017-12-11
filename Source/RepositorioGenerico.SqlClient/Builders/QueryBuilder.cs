using System;
using System.Collections.Generic;
using System.Text;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search;

namespace RepositorioGenerico.SqlClient.Builders
{
	public class QueryBuilder : IQueryBuilder
	{

		private int? _limite;
		internal int? Limites
		{
			get { return _limite; }
		}

		private string _tabela;
		internal string Tabela
		{
			get { return _tabela; }
		}

		private int _proximoParametro;
		internal int ProximoParametro
		{
			get { return _proximoParametro; }
		}


		private IList<string> _selects;
		internal IList<string> Selects
		{
			get { return _selects ?? (_selects = new List<string>()); }
		}

		private IList<string> _joins;
		internal IList<string> Joins
		{
			get { return _joins ?? (_joins = new List<string>()); }
		}

		private IList<string> _wheres;
		internal IList<string> Wheres
		{
			get { return _wheres ?? (_wheres = new List<string>()); }
		}

		private IList<string> _groupBys;
		internal IList<string> GroupBys
		{
			get { return _groupBys ?? (_groupBys = new List<string>()); }
		}

		private IList<string> _havings;
		internal IList<string> Havings
		{
			get { return _havings ?? (_havings = new List<string>()); }
		}

		private IList<string> _orderBys;
		internal IList<string> OrderBys
		{
			get { return _orderBys ?? (_orderBys = new List<string>()); }
		}

		public IQueryBuilder Criar()
		{
			return new QueryBuilder();
		}

		public void DefinirLimite(int limite)
		{
			_limite = limite;
		}

		public void AdicionarResultado(string campo)
		{
			Selects.Add("[" + campo + "]");
		}

		public void AdicionarResultadoPersonalizado(string resultado)
		{
			Selects.Add(resultado);
		}

		public void AdicionarResultadoAgregado(Agregadores agregador, string campo)
		{
			string conteudo;
			if (agregador == Agregadores.Count)
				conteudo = (campo == null)
					? "(*)"
					: string.Concat("(distinct [", campo, "])as[", campo, "]");
			else
				conteudo = string.Concat("([", campo, "])as[", campo, "]");
			Selects.Add(string.Concat(" ", agregador, conteudo));
		}

		public void DefinirTabela(string nome)
		{
			_tabela = nome;
		}

		public void AdicionarAgrupamento(string agrupamento)
		{
			GroupBys.Add(agrupamento);
		}

		public void AdicionarCondicaoAgrupamento(string condicao)
		{
			Havings.Add(string.Concat("(", condicao, ")"));
		}

		public void AdicionarCondicaoPersonalizada(string condicao)
		{
			Wheres.Add(string.Concat("(", condicao, ")"));
		}

		public string AdicionarCondicao(string campo, int operador, object valor)
		{
			var parametro = "_p" + _proximoParametro.ToString();
			if ((valor == null) || (valor == DBNull.Value))
				Wheres.Add(string.Concat("([", campo, "]IS NULL)"));
			else
				Wheres.Add(string.Concat("([", campo, "]", ConsultarSinalDoOperador(operador), "@", parametro, ")"));
			_proximoParametro++;
			return parametro;
		}

		public void AdicionarCondicaoApenasCampoNaoNulo(string campo)
		{
			Wheres.Add(string.Concat("([", campo, "]IS NOT NULL)"));
		}

		public string ConsultarOperador(Operadores operador)
		{
			return ConsultarSinalDoOperador(operador.GetHashCode());
		}

		public string ConsultarOperador(OperadoresTexto operador)
		{
			return ConsultarSinalDoOperador(operador.GetHashCode());
		}

		public string ConsultarOperador(OperadoresEspeciais operador)
		{
			return ConsultarSinalDoOperador(operador.GetHashCode());
		}

		private string ConsultarSinalDoOperador(int operador)
		{
			if (operador == (int)Operadores.Igual)
				return "=";
			if (operador == (int)Operadores.Diferente)
				return "<>";
			if (operador == (int)Operadores.Menor)
				return "<";
			if (operador == (int)Operadores.MenorOuIgual)
				return "<=";
			if (operador == (int)Operadores.Maior)
				return ">";
			if (operador == (int)Operadores.MaiorOuIgual)
				return ">=";
			if (operador == (int)OperadoresTexto.Contendo)
				return "like";
			if (operador == (int)OperadoresTexto.NaoContendo)
				return "not like";
			if (operador == (int)OperadoresEspeciais.In)
				return "in";
			if (operador == (int)OperadoresEspeciais.NotIn)
				return "not in";
			if (operador == (int)OperadoresEspeciais.Is)
				return "is";
			if (operador == (int)OperadoresEspeciais.IsNot)
				return "is not";
			return "=";
		}

		public string[] AdicionarCondicaoEntre(string campo, object inicio, object fim)
		{
			var parametros = new[] { string.Empty, string.Empty };
			var parametroInicio = string.Concat("_p", _proximoParametro.ToString(), "a");
			var parametroFim = string.Concat("_p", _proximoParametro.ToString(), "b");

			if ((inicio != null) && (fim != null))
			{
				parametros[0] = parametroInicio;
				parametros[1] = parametroFim;
				Wheres.Add(string.Concat("([", campo, "]between @", parametroInicio, " and @", parametroFim, ")"));
			}
			else if (inicio != null)
			{
				parametros[0] = parametroInicio;
				Wheres.Add(string.Concat("([", campo, "]>=@", parametroInicio, ")"));
			}
			else
			{
				parametros[1] = parametroFim;
				Wheres.Add(string.Concat("([", campo, "]<=@", parametroFim, ")"));
			}

			_proximoParametro++;
			return parametros;
		}

		public void AdicionarRelacionamento(string relacionamento)
		{
			Joins.Add(relacionamento);
		}

		public void AdicionarOrdem(string ordem)
		{
			OrderBys.Add(ordem);
		}

		public void AdicionarOrdemDescendente(string ordem)
		{
			OrderBys.Add(string.Concat(ordem, " DESC"));
		}

		public string GerarScript(Dicionario dicionario)
		{
			var sql = new StringBuilder();

			if (Selects.Count == 0) 
				ConstruirSelectPadrao(dicionario);

			sql.Append("select");

			var top = (_limite == null) 
				? string.Empty 
				: string.Concat(" top ", _limite.ToString(),  " ");

			AdicionarSql(sql, top, _selects, ",");
			sql.Append("from[");
			sql.Append(_tabela);
			sql.Append("]");
			AdicionarSql(sql, "", _joins, " ");
			AdicionarSql(sql, "where", _wheres, "and");
			AdicionarSql(sql, "group by ", _groupBys, ",");
			AdicionarSql(sql, "having", _havings, "and");
			AdicionarSql(sql, "order by ", _orderBys, ",");
			return sql.ToString();
		}

		public string GerarScriptExistencia(Dicionario dicionario)
		{
			var sql = new StringBuilder();
			sql.Append("select top 1 1 from[");
			sql.Append(_tabela);
			sql.Append("]");
			AdicionarSql(sql, "", _joins, " ");
			AdicionarSql(sql, "where", _wheres, "and");
			AdicionarSql(sql, "group by ", _groupBys, ",");
			AdicionarSql(sql, "having", _havings, "and");
			AdicionarSql(sql, "order by ", _orderBys, ",");
			return sql.ToString();
		}

		private void ConstruirSelectPadrao(Dicionario dicionario)
		{
			if (dicionario == null)
				Selects.Add(string.Concat("[", _tabela, "].*"));
			else
				foreach (var item in dicionario.Itens)
					Selects.Add(string.Concat("[", item.Nome, "]", ConsultarAliasDoCampo(item)));
		}

		private string ConsultarAliasDoCampo(ItemDicionario item)
		{
			if (string.IsNullOrEmpty(item.Alias))
				return string.Empty;
			return string.Concat("as[", item.Alias, "]");
		}

		private void AdicionarSql(StringBuilder sql, string antes, IList<string> lista, string separador)
		{
			if ((lista == null) || (lista.Count == 0))
				return;
			sql.Append(antes);
			foreach (var item in lista)
			{
				sql.Append(item);
				sql.Append(separador);
			}
			sql.Length -= separador.Length;
			sql.Append(" ");
		}

	}
}
