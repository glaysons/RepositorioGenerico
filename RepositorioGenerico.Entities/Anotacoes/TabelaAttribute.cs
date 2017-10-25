using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Class)]
	public class TabelaAttribute : Attribute
	{

		public string Nome { get; private set; }

		public TabelaAttribute(string nome)
		{
			Nome = nome;
		}

	}

}
