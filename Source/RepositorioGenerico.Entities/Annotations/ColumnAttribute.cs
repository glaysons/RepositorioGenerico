using RepositorioGenerico.Entities.Anotacoes;

namespace RepositorioGenerico.Entities.Annotations
{

	public class ColumnAttribute : ColunaAttribute
	{

		public string Name
		{
			get { return Nome; }
			set { Nome = value; }
		}

		public int Order
		{
			get { return Ordem; }
			set { Ordem = value; }
		}

		public string TypeName
		{
			get { return NomeDoTipo; }
			set { NomeDoTipo = value; }
		}

	}

}
