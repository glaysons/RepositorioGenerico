using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositorioGenerico.Dictionary.Itens
{
	public static class ItemDicionarioBuilder
	{
		public static IList<ItemDicionario> ConsultarItensDicionario(Type tipo)
		{
			var id = 0;
			var propriedades = tipo.GetProperties();
			var itens = from propriedade in propriedades
						select ItemDicionarioFactory.CriarItemDicionario(id++, propriedade);
			return itens.ToList();
		}
	}
}
