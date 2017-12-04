using Entities;
using RepositorioGenerico.Pattern.Contextos;
using System.Collections.Generic;

namespace Business.Clientes
{
	public class ConsultaClienteBusiness
	{

		private IRepositorio<Cliente> _repositorio;

		public ConsultaClienteBusiness(IRepositorio<Cliente> repositorio)
		{
			_repositorio = repositorio;
		}

		public IEnumerable<Cliente> ConsultarTodosOsClientes()
		{
			var config = _repositorio.Buscar.CriarQuery();
			config.CarregarPropriedade(c => c.Filhos);
			return _repositorio.Buscar.Varios(config);
		}

		public Cliente ConsultarCliente(int codigo)
		{
			return _repositorio.Consultar(codigo);
		}

		public Cliente ConsultarClienteComFilhosEContatos(int codigo)
		{
			var config = _repositorio.Buscar.CriarQuery()
				.CarregarPropriedade(c => c.Filhos)
				.CarregarPropriedade(c => c.Contatos)
				.AdicionarCondicao(c => c.Id).Igual(codigo);
			return _repositorio.Buscar.Um(config);
		}

		public bool ExisteClienteCadastrado(Cliente cliente)
		{
			var config = _repositorio.Buscar.CriarQuery()
				.AdicionarCondicao(c => c.Id).Diferente(cliente.Id)
				.AdicionarCondicao(c => c.Nome).Igual(cliente.Nome);
			return _repositorio.Buscar.Existe(config);
		}

	}
}
