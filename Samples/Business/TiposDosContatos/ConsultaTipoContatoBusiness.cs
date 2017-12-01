using Entities;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Pattern.Contextos;
using System.Collections.Generic;

namespace Business.TiposDosContatos
{
	public class ConsultaTipoContatoBusiness
	{

		private IRepositorio<TipoContato> _repositorio;

		public ConsultaTipoContatoBusiness(IRepositorio<TipoContato> repositorio)
		{
			_repositorio = repositorio;
		}

		public IEnumerable<TipoContato> ConsultarTodosOsTiposDosContatos()
		{
			return _repositorio.Buscar.Todos();
		}

		public TipoContato ConsultarTipoContato(int codigo)
		{
			var config = _repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Id).Igual(codigo);
			return _repositorio.Buscar.Um(config);
		}

		public bool ExisteTipoContatoCadastrado(TipoContato tipoContato)
		{
			var config = _repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Id).Diferente(tipoContato.Id)
				.AdicionarCondicao(c => c.Nome).Igual(tipoContato.Nome);
			return _repositorio.Buscar.Existe(config);
		}

		public bool ExistemContatosVinculados(TipoContato TipoContato)
		{
			var config = _repositorio.Buscar.CriarProcedure("spExistemContatosVinculados");
			config.DefinirParametro(c => c.Id).Valor(TipoContato.Id);
			return _repositorio.Buscar.Existe(config);
		}

	}
}
