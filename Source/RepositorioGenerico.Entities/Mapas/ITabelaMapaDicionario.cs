using System;
using System.Linq.Expressions;

namespace RepositorioGenerico.Entities.Mapas
{
	public interface ITabelaMapaDicionario<TModel, TTabela>
		where TModel : class, IEntidade
		where TTabela : class, IEntidade
	{
		ICampoMapaDicionario<TModel, TTabela> Campo(Expression<Func<TTabela, object>> campo);
	}
}
