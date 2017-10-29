using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RepositorioGenerico.Entities;

namespace RepositorioGenerico.Pattern.Buscadores
{
	public interface IBuscador<TObjeto>
	{

		IConfiguracaoProcedure<TObjeto> CriarProcedure();

		IConfiguracaoProcedure<TObjeto> CriarProcedure(string nome);

		IConfiguracaoQuery<TObjeto> CriarQuery();

		IEnumerable<TObjeto> Todos();

		IEnumerable<TObjeto> Varios(IConfiguracao<TObjeto> configuracao);

		TObjeto Um(IConfiguracao<TObjeto> configuracao);

		object Scalar(IConfiguracao<TObjeto> configuracao);

		int NonQuery(IConfiguracao<TObjeto> configuracao);

		bool Existe(IConfiguracao<TObjeto> configuracao);

		TOutroObjeto ConsultarPropriedade<TOutroObjeto>(TObjeto objeto, Expression<Func<TObjeto, TOutroObjeto>> propriedade) where TOutroObjeto : class, IEntidade;

		ICollection<TEstadoObjeto> ConsultarPropriedade<TEstadoObjeto>(TObjeto objeto, Expression<Func<TObjeto, ICollection<TEstadoObjeto>>> propriedade) where TEstadoObjeto : class, IEntidade;

	}
}
