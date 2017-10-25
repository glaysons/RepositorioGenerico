using System;

namespace RepositorioGenerico.Entities.Anotacoes
{

	[AttributeUsage(AttributeTargets.Property)]
	public class TamanhoMaximoAttribute : Attribute
	{

		public int Tamanho { get; private set; }

		public TamanhoMaximoAttribute(int tamanho)
		{
			Tamanho = tamanho;
		}

	}

}
