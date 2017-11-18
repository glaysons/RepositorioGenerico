using System.Collections.Generic;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.Fake.Contextos
{
	public interface IContextoFake : IContexto
	{

		void AdicionarRegistros<TObjeto>(IList<TObjeto> registros) where TObjeto : IEntidade;

		void AdicionarRegistro<TObjeto>(TObjeto registro) where TObjeto : IEntidade;

		void DefinirResultadoProcedure<TObjeto>(string nome, IList<TObjeto> registros) where TObjeto : IEntidade;

		void DefinirResultadoScalarProcedure(string nome, object valor);

		void DefinirResultadoNonQueryProcedure(string nome, int registrosAfetados);

	}
}
