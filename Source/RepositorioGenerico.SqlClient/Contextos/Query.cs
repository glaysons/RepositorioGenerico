using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RepositorioGenerico.Search.Conversores;
using RepositorioGenerico.SqlClient.Builders;

namespace RepositorioGenerico.SqlClient.Contextos
{
	public class Query<TObjeto> : IOrderedQueryable<TObjeto>, IQueryProvider
	{

		private readonly Comando _comando;
		private Expression _expression = null;

		public Query(Comando comando)
		{
			_comando = comando;
		}

		public IEnumerator<TObjeto> GetEnumerator()
		{
			return Execute<IList<TObjeto>>(_expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Expression Expression
		{
			get { return Expression.Constant(this); }
		}

		public Type ElementType
		{
			get { return typeof(TObjeto); }
		}

		public IQueryProvider Provider
		{
			get { return this; }
		}

		public IQueryable CreateQuery(Expression expression)
		{
			return CreateQuery<TObjeto>(expression);
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			var comando = _comando.CriarComando();
			var sql = new QueryBuilder<TElement>(expression).ToString();
			comando.CommandText = sql;
			var reader = _comando.ConsultarRegistro(comando);
			var itens = new List<TElement>();
			try
			{
				var conversor = Conversor.ConverterDataReaderParaObjeto<TElement>(reader);
				foreach (var registro in conversor)
					itens.Add(registro);
			}
			finally
			{
				reader.Close();
			}
			return itens.AsQueryable();
		}

		public object Execute(Expression expression)
		{
			return Execute<TObjeto>(expression);
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return CreateQuery<TResult>(expression).SingleOrDefault();
		}
	}
}
