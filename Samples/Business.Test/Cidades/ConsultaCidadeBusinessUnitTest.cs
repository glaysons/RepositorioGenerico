using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using FluentAssertions;

namespace Business.Test.Cidades
{
	[TestClass]
	public class ConsultaCidadeBusinessUnitTest
	{

		[TestMethod]
		public void SeConsultarTodasAsCidadesDeveEncontrarApenasOsRegistrosConfiguradosNaFactory()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();

			consultador.ConsultarTodasAsCidades()
				.Should()
				.HaveCount(6);

			factory.Repositorio.Quantidade
				.Should().Be(0);
		}

		[TestMethod]
		public void SeExistirUmaCidadeCadastradaDeveRetornarVerdadeiro()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();

			var cidade = new Cidade()
			{
				Nome = "Cuiabá",
				Estado = "MT"
			};

			consultador.ExisteCidadeCadastrada(cidade)
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeNaoExistirUmaCidadeCadastradaDeveRetornarFalso()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();

			var cidade = new Cidade()
			{
				Nome = "Sinop",
				Estado = "MT"
			};

			consultador.ExisteCidadeCadastrada(cidade)
				.Should()
				.BeFalse();
		}

		[TestMethod]
		public void SeExistemClientesVinculadosDeveRetornarVerdadeiro()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();

			factory.ExistemClientesVinculados(existe: true);

			var cidade = new Cidade() {
				Id = 1,
				Nome = "Cuiabá",
				Estado = "MT"
			};

			consultador.ExistemClientesVinculados(cidade)
				.Should()
				.BeTrue();
		}

		[TestMethod]
		public void SeNaoExistemClientesVinculadosDeveRetornarFalso()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();

			factory.ExistemClientesVinculados(existe: false);

			var cidade = new Cidade()
			{
				Id = 1,
				Nome = "Cuiabá",
				Estado = "MT"
			};

			consultador.ExistemClientesVinculados(cidade)
				.Should()
				.BeFalse();
		}

		[TestMethod]
		public void SeConsultarCidadeExistentePorCodigoDeveEncontrar()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();

			var cidade = consultador.ConsultarCidade(1);

			cidade
				.Should().NotBeNull();

			cidade.Nome
				.Should().Be("Cuiabá");
		}

		[TestMethod]
		public void SeConsultarUmaCidadeInexistentePorCodigoDeveRetornarNulo()
		{
			var factory = new CidadeFactory();
			var consultador = factory.CriarConsultador();

			var cidade = consultador.ConsultarCidade(99);

			cidade
				.Should().BeNull();
		}

	}
}
