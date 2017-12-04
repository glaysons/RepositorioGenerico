using System;
using System.ComponentModel;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class DescricaoAttribute : DisplayNameAttribute
	{

		public string Descricao { get; private set; }

		public DescricaoAttribute(string descricao)
			: base(descricao)
		{
			Descricao = descricao;
		}
	}

}
