using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities.Annotations
{

	public class MaxLengthAttribute : TamanhoMaximoAttribute
	{

		public int Length { get { return Tamanho; } }

		public MaxLengthAttribute(int length)
			: base(length)
		{

		}

	}

}
