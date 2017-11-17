using RepositorioGenerico.Entities;

namespace RepositorioGenerico.Test.Objetos
{
	public class FilhoMapeadoDoObjetoMapeadoDeTestes : Entidade
	{

		public int MapeadoComCodigoFilho { get; set; }

		public string MapeadoComNomeFilho { get; set; }

		public int MapeadoComCodigoPai { get; set; }

		public virtual ObjetoMapeadoDeTestes MapeadoComPai { get; set; }

	}
}
