using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property)]
	public class ChaveEstrangeiraAttribute : Attribute
	{

		public string Nome { get; private set; }

		public ChaveEstrangeiraAttribute(string nome)
		{
			Nome = nome;
		}

	}

}
