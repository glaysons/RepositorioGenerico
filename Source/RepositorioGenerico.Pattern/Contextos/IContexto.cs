using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Pattern.Contextos
{
	public interface IContexto
	{

		IRepositorio<TObjeto> Repositorio<TObjeto>() where TObjeto : IEntidade;

		IBuscador<TObjeto> Buscar<TObjeto>();

		void Salvar();

	}
}
