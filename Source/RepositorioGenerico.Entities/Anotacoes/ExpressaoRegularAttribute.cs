using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property)]
	public class ExpressaoRegularAttribute : Attribute
	{

		public string Expressao { get; private set; }

		public ExpressaoRegularAttribute(string expressao)
		{
			Expressao = expressao;
		}
	}

}
