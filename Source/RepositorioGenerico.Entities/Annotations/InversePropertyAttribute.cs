using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities.Annotations
{

	public class InversePropertyAttribute : PropriedadeDeLigacaoEstrangeiraAttribute
	{

		public string Name { get { return Nome; } }

		public InversePropertyAttribute(string name)
			: base(name)
		{
		}

	}

}
