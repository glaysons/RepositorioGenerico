using System;
using System.Collections.Generic;
using RepositorioGenerico.Entities;

namespace RepositorioGenerico.Dictionary
{
	public static class EstadoObjetoFactory
	{

		public static TObjeto Criar<TObjeto>(this ICollection<TObjeto> sender) where TObjeto : IEntidade
		{
			return Criar<TObjeto>();
		}

		public static TObjeto Criar<TObjeto>() where TObjeto : IEntidade
		{
			var objeto = (TObjeto) Activator.CreateInstance(typeof (TObjeto));
			var dicionario = DicionarioCache.Consultar(typeof (TObjeto));
			foreach (var item in dicionario.Itens)
				if (item.ValorPadrao != null)
					item.Propriedade.SetValue(objeto, item.ValorPadrao, null);
			if (dicionario.PossuiCamposFilho)
				foreach (var filho in dicionario.ConsultarCamposFilho())
				{
					var tipo = typeof (List<>).MakeGenericType(filho.Ligacao.Dicionario.Tipo);
					filho.Propriedade.SetValue(objeto, Activator.CreateInstance(tipo), null);
				}
			return objeto;
		}
	}
}
