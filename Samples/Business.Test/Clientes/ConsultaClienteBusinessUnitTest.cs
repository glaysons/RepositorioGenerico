using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using FluentAssertions;

namespace Business.Test.Clientes
{
	[TestClass]
	public class ConsultaClienteBusinessUnitTest
	{

		[TestMethod]
		public void SeConsultarTodosOsClientesDeveEncontrarApenasOsRegistrosConfiguradosNaFactory()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();

			consultador.ConsultarTodosOsClientes()
				.Should()
				.HaveCount(3);

			factory.Repositorio.Quantidade
				.Should().Be(0);
		}

		[TestMethod]
		public void SeExistirUmClienteCadastroDeveRetornarVerdadeiro()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();

			var cliente = new Cliente()
			{
				Nome = "João Abc da Silva"
			};

			consultador.ExisteClienteCadastrado(cliente)
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeNaoExistirUmClienteCadastradoDeveRetornarFalso()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();

			var cliente = new Cliente()
			{
				Nome = "Francisco dos Santos"
			};

			consultador.ExisteClienteCadastrado(cliente)
				.Should()
				.BeFalse();
		}

		[TestMethod]
		public void SeConsultarClienteExistentePorCodigoDeveEncontrar()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();

			var cliente = consultador.ConsultarCliente(1);

			cliente
				.Should().NotBeNull();

			cliente.Nome
				.Should().Be("João Abc da Silva");

			cliente.Filhos
				.Should()
				.BeNull();

			cliente.Contatos
				.Should()
				.BeNull();
		}

		[TestMethod]
		public void SeConsultarUmClienteInexistentePorCodigoDeveRetornarNulo()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();

			var Cliente = consultador.ConsultarCliente(99);

			Cliente
				.Should().BeNull();
		}

		[TestMethod]
		public void SeConsultarClienteExistenteCarregandoOsFilhosPorCodigoDeveEncontrar()
		{
			var factory = new ClienteFactory();
			var consultador = factory.CriarConsultador();

			var cliente = consultador.ConsultarClienteComFilhosEContatos(1);

			cliente
				.Should().NotBeNull();

			cliente.Nome
				.Should().Be("João Abc da Silva");

			cliente.Filhos
				.Should()
				.HaveCount(2);

			cliente.Contatos
				.Should()
				.HaveCount(1);
		}

	}
}
