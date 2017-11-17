using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities.Annotations
{

	public class ForeignKeyAttribute : ChaveEstrangeiraAttribute
	{

		public string Name { get { return Nome; } }

		public ForeignKeyAttribute(string name)
			: base(name)
		{
		}

	}

}
