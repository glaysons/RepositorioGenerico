using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class DescricaoAttribute : Attribute
	{

		public string Descricao { get; private set; }

		public DescricaoAttribute(string descricao)
		{
			Descricao = descricao;
		}
	}

}
