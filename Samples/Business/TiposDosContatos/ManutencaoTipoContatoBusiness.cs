using Entities;
using RepositorioGenerico.Pattern.Contextos;
using System;

namespace Business.TiposDosContatos
{
	public class ManutencaoTipoContatoBusiness
	{

		private IRepositorio<TipoContato> _repositorio;
		private ConsultaTipoContatoBusiness _consultador;

		public ManutencaoTipoContatoBusiness(IRepositorio<TipoContato> repositorio, ConsultaTipoContatoBusiness consultador)
		{
			_repositorio = repositorio;
			_consultador = consultador;
		}

		public void Cadastrar(TipoContato tipoContato)
		{
			ValidarTipoContatoCadastrado(tipoContato);
			_repositorio.Inserir(tipoContato);
		}

		private void ValidarTipoContatoCadastrado(TipoContato tipoContato)
		{
			if (_consultador.ExisteTipoContatoCadastrado(tipoContato))
				throw new Exception("Já existe um Tipo de Contato cadastrado com este nome!");
		}

		public void Atualizar(TipoContato tipoContato)
		{
			ValidarTipoContatoCadastrado(tipoContato);
			_repositorio.Atualizar(tipoContato);
		}

		public void Excluir(TipoContato tipoContato)
		{
			if (_consultador.ExistemContatosVinculados(tipoContato))
				throw new Exception("Ainda existem contatos vinculados com este Tipo de Contato!");
			_repositorio.Excluir(tipoContato);
		}

	}
}
