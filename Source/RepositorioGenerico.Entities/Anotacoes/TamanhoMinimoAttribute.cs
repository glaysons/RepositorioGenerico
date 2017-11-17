using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property)]
	public class TamanhoMinimoAttribute : Attribute
	{

		public int Tamanho { get; private set; }

		public TamanhoMinimoAttribute(int tamanho)
		{
			Tamanho = tamanho;
		}
	}

}
