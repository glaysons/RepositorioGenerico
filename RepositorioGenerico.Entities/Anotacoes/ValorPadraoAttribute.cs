using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ValorPadraoAttribute : Attribute
	{

		public object Valor { get; private set; }

		public ValorPadraoAttribute(object valor)
		{
			Valor = valor;
		}
	}

}
