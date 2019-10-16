using RepositorioGenerico.Entities;
using RepositorioGenerico.Pattern.Buscadores;
using System;

namespace RepositorioGenerico.Pattern.Contextos
{
	public interface IContexto : IDisposable
	{

		bool LimparContextoAoSalvar { get; set; }

		IRepositorio<TObjeto> Repositorio<TObjeto>() where TObjeto : IEntidade;

		IBuscador<TObjeto> Buscar<TObjeto>();

		void Salvar();

		void Limpar();

	}
}
