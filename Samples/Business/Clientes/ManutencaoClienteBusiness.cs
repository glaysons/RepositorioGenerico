using Entities;
using RepositorioGenerico.Pattern.Contextos;
using System;

namespace Business.Clientes
{
	public class ManutencaoClienteBusiness
	{

		private IRepositorio<Cliente> _repositorio;
		private ConsultaClienteBusiness _consultador;

		public ManutencaoClienteBusiness(IRepositorio<Cliente> repositorio, ConsultaClienteBusiness consultador)
		{
			_repositorio = repositorio;
			_consultador = consultador;
		}

		public void Cadastrar(Cliente cliente)
		{
			ValidarClienteCadastrado(cliente);
			_repositorio.Inserir(cliente);
		}

		private void ValidarClienteCadastrado(Cliente cliente)
		{
			if (_consultador.ExisteClienteCadastrado(cliente))
				throw new Exception("Já existe um Cliente cadastrado com este nome!");
		}

		public void Atualizar(Cliente cliente)
		{
			ValidarClienteCadastrado(cliente);
			_repositorio.Atualizar(cliente);
		}

		public void Excluir(Cliente cliente)
		{
			_repositorio.Excluir(cliente);
		}

	}
}
