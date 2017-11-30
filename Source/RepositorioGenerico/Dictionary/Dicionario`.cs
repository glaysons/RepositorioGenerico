using RepositorioGenerico.Entities;

namespace RepositorioGenerico.Dictionary
{
	public class Dicionario<TObjeto> : Dicionario where TObjeto : IEntidade
	{

		public Dicionario()
			: base(typeof(TObjeto))
		{

		}

	}
}
