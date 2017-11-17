using System.Collections.Generic;
using RepositorioGenerico.Dictionary.Itens;
using RepositorioGenerico.Dictionary.Relacionamentos;

namespace RepositorioGenerico.Search
{
	public interface IRelacionamentoBuilder
	{

		string CriarScriptConsultaRelacionamentoAscendente(Relacionamento relacionamento, string condicao);

		string CriarScriptConsultaRelacionamentoDescendente(Relacionamento relacionamento, string condicao,
			IEnumerable<ItemDicionario> camposChave);

	}
}
