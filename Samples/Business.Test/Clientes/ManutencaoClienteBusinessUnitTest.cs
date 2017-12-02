using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using FluentAssertions;
using RepositorioGenerico.Entities;

namespace Business.Test.Clientes
{
	[TestClass]
	public class ManutencaoClienteBusinessUnitTest
	{

		[TestMethod]
		public void SeCadastrarUmClienteNaoDeveGerarErro()
		{
			var factory = new ClienteFactory();
			var manutencao = factory.CriarManutencao();

			var Cliente = new Cliente()
			{
				Nome = "Francisco dos Santos"
			};

			Action cadastro = () => manutencao.Cadastrar(Cliente);

			cadastro
				.ShouldNotThrow();
		}

		[TestMethod]
		public void SeCadastrarUmClienteDeveSerPossivelConsultalo()
		{
			var factory = new ClienteFactory();
			var manutencao = factory.CriarManutencao();

			var Cliente = new Cliente()
			{
				Nome = "Francisco dos Santos"
			};

			factory.Repositorio.Quantidade
				.Should().Be(0);

			manutencao.Cadastrar(Cliente);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeCadastrarUmClienteExistenteDeveGerarErro()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			var Cliente = new Cliente()
			{
				Nome = "João Abc da Silva"
			};

			Action cadastro = () => manutencao.Cadastrar(Cliente);

			cadastro
				.ShouldThrow<Exception>()
				.WithMessage("Já existe um Cliente cadastrado com este nome!");

		}

		[TestMethod]
		public void SeAlterarUmClienteExistenteDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			var Cliente = consultador.ConsultarCliente(1);

			Cliente
				.Should().NotBeNull();

			Cliente.Nome = "Francisco dos Santos";

			manutencao.Atualizar(Cliente);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeAlterarUmClienteExistenteSemExecutarAConsultaDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new ClienteFactory();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			var Cliente = new Cliente()
			{
				Id = 1,
				Nome = "Francisco dos Santos",
				EstadoEntidade = EstadosEntidade.Novo
			};

			manutencao.Atualizar(Cliente);

			Cliente.EstadoEntidade
				.Should().Be(EstadosEntidade.Modificado);

			factory.Repositorio.Quantidade
				.Should().Be(1);
		}

		[TestMethod]
		public void SeExcluirUmClienteExistenteDeveEntrarNaListaDeAtualizacao()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();
			var manutencao = factory.CriarManutencao();

			factory.Repositorio.Quantidade
				.Should().Be(0);

			var Cliente = consultador.ConsultarCliente(1);

			Cliente
				.Should().NotBeNull();

			manutencao.Excluir(Cliente);

			factory.Repositorio.Quantidade
				.Should().Be(0);
		}

	}
}
