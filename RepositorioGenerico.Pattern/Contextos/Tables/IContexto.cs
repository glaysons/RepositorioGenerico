using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Pattern.Contextos.Tables
{
	public interface IContexto
	{

		IRepositorio Repositorio<TObjeto>() where TObjeto : class, IEntidade;

		IBuscador Buscar();

	}
}
