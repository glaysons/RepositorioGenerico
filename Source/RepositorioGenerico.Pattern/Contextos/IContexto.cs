using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Buscadores;
using System;

namespace RepositorioGenerico.Pattern.Contextos
{
	public interface IContexto : IDisposable
	{

		IRepositorio<TObjeto> Repositorio<TObjeto>() where TObjeto : IEntidade;

		IBuscador<TObjeto> Buscar<TObjeto>();

		void Salvar();

	}
}
