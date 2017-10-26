using System.Collections.Generic;
using RepositorioGenerico.Dictionary.Itens;

namespace RepositorioGenerico.Dictionary
{
	public interface IDicionario
	{
		IEnumerable<ItemDicionario> Itens { get; }
		IEnumerable<ItemDicionario> ConsultarCamposChave();
		ItemDicionario ConsultarPorCampo(string nome);
		ItemDicionario ConsultarPorPropriedade(string nome);
	}
}