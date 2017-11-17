using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities.Annotations
{

	public class DescriptionAttribute : DescricaoAttribute
	{

		public string Description { get { return Descricao; } }

		public DescriptionAttribute(string description)
			: base(description)
		{

		}

	}

}
