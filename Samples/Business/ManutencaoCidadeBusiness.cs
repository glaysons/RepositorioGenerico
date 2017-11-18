using Entities;
using RepositorioGenerico.Pattern.Contextos;
using System;

namespace Business
{
	public class ManutencaoCidadeBusiness
	{

		private IRepositorio<Cidade> _repositorio;
		private ConsultaCidadeBusiness _consultador;

		public ManutencaoCidadeBusiness(IRepositorio<Cidade> repositorio, ConsultaCidadeBusiness consultador)
		{
			_repositorio = repositorio;
			_consultador = consultador;
		}

		public void Cadastrar(Cidade cidade)
		{
			ValidarCidadeCadastrada(cidade);
			_repositorio.Inserir(cidade);
		}

		private void ValidarCidadeCadastrada(Cidade cidade)
		{
			if (_consultador.ExisteCidadeCadastrada(cidade))
				throw new Exception("Já existe uma cidade cadastrada com este nome!");
		}

		public void Atualizar(Cidade cidade)
		{
			ValidarCidadeCadastrada(cidade);
			_repositorio.Atualizar(cidade);
		}

		public void Excluir(Cidade cidade)
		{
			if (_consultador.ExistemClientesVinculados(cidade))
				throw new Exception("Ainda existem clientes cadastrados para esta cidade!");
			_repositorio.Excluir(cidade);
		}

	}
}
