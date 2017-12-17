using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Buscadores;
using System;

namespace RepositorioGenerico.Pattern.Contextos.Tables
{
	public interface IContexto : IDisposable
	{

		IRepositorio Repositorio<TObjeto>() where TObjeto : class, IEntidade;

		IBuscador Buscar();

	}
}
