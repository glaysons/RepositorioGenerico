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
			if (agregador == Agregadores.Count)
				if (campo == null)
					Selects.Add(" " + agregador.ToString() + "(*)");
				else
					Selects.Add(" " + agregador.ToString() + "(distinct [" + campo + "])as[" + campo + "]");
			else
				Selects.Add(" " + agregador.ToString() + "([" + campo + "])as[" + campo + "]");
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
			Havings.Add("(" + condicao + ")");
		}

		public void AdicionarCondicaoPersonalizada(string condicao)
		{
			Wheres.Add("(" + condicao + ")");
		}

		public string AdicionarCondicao(string campo, int operador, object valor)
		{
			var parametro = "_p" + _proximoParametro.ToString();
			if ((valor == null) || (valor == DBNull.Value))
				Wheres.Add("([" + campo + "]IS NULL)");
			else
				Wheres.Add("([" + campo + "]" + ConsultarSinalDoOperador(operador) + "@" + parametro + ")");
			_proximoParametro++;
			return parametro;
		}

		public void AdicionarCondicaoApenasCampoNaoNulo(string campo)
		{
			Wheres.Add("([" + campo + "]IS NOT NULL)");
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
				return "like ";
			if (operador == (int)OperadoresTexto.NaoContendo)
				return "not like ";
			if (operador == (int)OperadoresEspeciais.In)
				return "in ";
			if (operador == (int)OperadoresEspeciais.NotIn)
				return "not in ";
			if (operador == (int)OperadoresEspeciais.Is)
				return "is ";
			if (operador == (int)OperadoresEspeciais.IsNot)
				return "is not ";
			return "=";
		}

		public string[] AdicionarCondicaoEntre(string campo, object inicio, object fim)
		{
			var parametros = new[] { string.Empty, string.Empty };
			var parametroInicio = "_p" + _proximoParametro.ToString() + "a";
			var parametroFim = "_p" + _proximoParametro.ToString() + "b";

			if ((inicio != null) && (fim != null))
			{
				parametros[0] = parametroInicio;
				parametros[1] = parametroFim;
				Wheres.Add("([" + campo + "]between @" + parametroInicio + " and @" + parametroFim + ")");
			}
			else if (inicio != null)
			{
				parametros[0] = parametroInicio;
				Wheres.Add("([" + campo + "]>=@" + parametroInicio + ")");
			}
			else
			{
				parametros[1] = parametroFim;
				Wheres.Add("([" + campo + "]<=@" + parametroFim + ")");
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
			OrderBys.Add(ordem + " DESC");
		}

		public string GerarScript(Dicionario dicionario)
		{
			var sql = new StringBuilder();

			ConstruirSelectPadrao(dicionario);
			GerarSelectDoScript(sql);
			GerarFromDoScript(sql);
			GerarCorpoDoScript(sql);

			return sql.ToString();
		}

		private void ConstruirSelectPadrao(Dicionario dicionario)
		{
			if (_selects?.Count > 0)
				return;
			if (dicionario == null)
				Selects.Add("[" + _tabela + "].*");
			else
				foreach (var item in dicionario.Itens)
					Selects.Add("[" + item.Nome + "]" + ConsultarAliasDoCampo(item));
		}

		private string ConsultarAliasDoCampo(ItemDicionario item)
		{
			if (string.IsNullOrEmpty(item.Alias))
				return string.Empty;
			return "as[" + item.Alias + "]";
		}

		private void GerarCorpoDoScript(StringBuilder sql)
		{
			GerarJoinDoScript(sql);
			GerarWhereDoScript(sql);
			GerarGroupByDoScript(sql);
			GerarHavingDoScript(sql);
			GerarOrderByDoScript(sql);
		}

		private void GerarSelectDoScript(StringBuilder sql)
		{
			sql.Append("select ");

			if (_limite.HasValue)
			{
				sql.Append("top ");
				sql.Append(_limite);
				sql.Append(" ");
			}

			foreach (var item in Selects)
			{
				sql.Append(item);
				sql.Append(",");
			}

			sql.Length -= 1;

			sql.Append(" ");
		}

		private void GerarFromDoScript(StringBuilder sql)
		{
			sql.Append("from[");
			sql.Append(_tabela);
			sql.Append("]");
		}

		private void GerarJoinDoScript(StringBuilder sql)
		{
			if (!(_joins?.Count > 0))
				return;

			foreach (var item in _joins)
			{
				sql.Append(item);
				sql.Append(" ");
			}
		}

		private void GerarWhereDoScript(StringBuilder sql)
		{
			if (!(_wheres?.Count > 0))
				return;

			sql.Append("where");

			foreach (var item in _wheres)
			{
				sql.Append(item);
				sql.Append("and");
			}

			sql.Length -= 3;

			sql.Append(" ");
		}

		private void GerarGroupByDoScript(StringBuilder sql)
		{
			if (!(_groupBys?.Count > 0))
				return;

			sql.Append("group by ");

			foreach (var item in _groupBys)
			{
				sql.Append(item);
				sql.Append(",");
			}

			sql.Length -= 1;

			sql.Append(" ");
		}

		private void GerarHavingDoScript(StringBuilder sql)
		{
			if (!(_havings?.Count > 0))
				return;

			sql.Append("having");

			foreach (var item in _havings)
			{
				sql.Append(item);
				sql.Append("and");
			}

			sql.Length -= 3;

			sql.Append(" ");
		}

		private void GerarOrderByDoScript(StringBuilder sql)
		{
			if (!(_orderBys?.Count > 0))
				return;

			sql.Append("order by ");

			foreach (var item in _orderBys)
			{
				sql.Append(item);
				sql.Append(",");
			}

			sql.Length -= 1;

			sql.Append(" ");
		}

		public string GerarScriptExistencia(Dicionario dicionario)
		{
			var sql = new StringBuilder();
			sql.Append("select top 1 1 ");

			GerarFromDoScript(sql);
			GerarCorpoDoScript(sql);

			return sql.ToString();
		}

	}
}
