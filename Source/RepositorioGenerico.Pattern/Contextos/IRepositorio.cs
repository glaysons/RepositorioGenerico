using System.Collections.Generic;
using System.Linq;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Buscadores;

namespace RepositorioGenerico.Pattern.Contextos
{
	public interface IRepositorio<TObjeto> : IRepositorioBase<IList<TObjeto>, TObjeto> where TObjeto : IEntidade
	{

		IBuscador<TObjeto> Buscar { get; }

		IQueryable<TObjeto> Query { get; }

		bool SalvarFilhos { get; set; }

		void Excluir(IList<TObjeto> objeto);

	}
}
