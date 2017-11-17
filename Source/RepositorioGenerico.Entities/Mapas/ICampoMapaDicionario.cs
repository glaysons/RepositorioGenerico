using System;
using System.Linq.Expressions;

namespace RepositorioGenerico.Entities.Mapas
{
	public interface ICampoMapaDicionario<TModel, TTabela>
		where TModel : class, IEntidade
		where TTabela : class, IEntidade
	{

		ITabelaMapaDicionario<TModel, TTabela> Propriedade(Expression<Func<TModel, object>> propriedade);

	}
}
