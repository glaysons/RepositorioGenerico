using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using FluentAssertions;

namespace Business.Test.TiposDosContatos
{
	[TestClass]
	public class ConsultaTipoContatoBusinessUnitTest
	{

		[TestMethod]
		public void SeConsultarTodosOsTiposDosContatosDeveEncontrarApenasOsRegistrosConfiguradosNaFactory()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();

			consultador.ConsultarTodosOsTiposDosContatos()
				.Should()
				.HaveCount(6);

			factory.Repositorio.Quantidade
				.Should().Be(0);
		}

		[TestMethod]
		public void SeExistirUmTipoContatoCadastroDeveRetornarVerdadeiro()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();

			var tipoContato = new TipoContato()
			{
				Nome = "Telefone"
			};

			consultador.ExisteTipoContatoCadastrado(tipoContato)
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeNaoExistirUmTipoContatoCadastradoDeveRetornarFalso()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();

			var tipoContato = new TipoContato()
			{
				Nome = "Fumaça"
			};

			consultador.ExisteTipoContatoCadastrado(tipoContato)
				.Should()
				.BeFalse();
		}

		[TestMethod]
		public void SeExistemContatosVinculadosDeveRetornarVerdadeiro()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();

			factory.ExistemContatosVinculados(existe: true);

			var tipoContato = new TipoContato() {
				Id = 1,
				Nome = "Cuiabá"
			};

			consultador.ExistemContatosVinculados(tipoContato)
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeNaoExistemContatosVinculadosDeveRetornarFalso()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();

			factory.ExistemContatosVinculados(existe: false);

			var tipoContato = new TipoContato()
			{
				Id = 1,
				Nome = "Cuiabá"
			};

			consultador.ExistemContatosVinculados(tipoContato)
				.Should()
				.BeFalse();
		}

		[TestMethod]
		public void SeConsultarTipoContatoExistentePorCodigoDeveEncontrar()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();

			var TipoContato = consultador.ConsultarTipoContato(1);

			TipoContato
				.Should().NotBeNull();

			TipoContato.Nome
				.Should().Be("Telefone");
		}

		[TestMethod]
		public void SeConsultarUmTipoContatoInexistentePorCodigoDeveRetornarNulo()
		{
			var factory = new TipoContatoFactory();
			var consultador = factory.CriarConsultador();

			var TipoContato = consultador.ConsultarTipoContato(99);

			TipoContato
				.Should().BeNull();
		}

	}
}
