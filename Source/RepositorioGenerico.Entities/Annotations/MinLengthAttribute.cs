using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities.Annotations
{

	public class MinLengthAttribute : TamanhoMinimoAttribute
	{

		public int Length { get { return Tamanho; } }

		public MinLengthAttribute(int length)
			: base(length)
		{
		}

	}

}
