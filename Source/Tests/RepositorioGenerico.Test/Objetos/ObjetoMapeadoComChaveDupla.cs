using RepositorioGenerico.Entities;

namespace RepositorioGenerico.Test.Objetos
{
	public class ObjetoMapeadoComChaveDupla : Entidade
	{

		public int MapeadoComChaveBase { get; set; }

		public int MapeadoComChaveAutoIncremento { get; set; }

		public string MapeadoComNome { get; set; }

	}
}
