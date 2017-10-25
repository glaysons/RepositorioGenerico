using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property)]
	public class PropriedadeDeLigacaoEstrangeiraAttribute : Attribute
	{

		public string Nome { get; private set; }

		public PropriedadeDeLigacaoEstrangeiraAttribute(string nome)
		{
			Nome = nome;
		}

	}

}
