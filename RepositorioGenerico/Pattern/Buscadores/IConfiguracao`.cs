using System;
using System.Linq.Expressions;

namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IConfiguracao<TObjeto> : IConfiguracao
	{
		IConfiguracaoParametro<TObjeto> DefinirParametro(Expression<Func<TObjeto, object>> nome);
	}
}
