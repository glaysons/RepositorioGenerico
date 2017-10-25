using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities.Annotations
{

	public class TableAttribute : TabelaAttribute
	{

		public string Name { get { return Nome; } }

		public TableAttribute(string name)
			: base(name)
		{

		}

	}

}
