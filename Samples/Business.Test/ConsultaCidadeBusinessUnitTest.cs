using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entities;
using FluentAssertions;

namespace Business.Test
{
	[TestClass]
	public class ConsultaCidadeBusinessUnitTest
	{
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

			factory.ExistemClientesVinculados(consultador, true);

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

			factory.ExistemClientesVinculados(consultador, false);

			var cidade = new Cidade()
			{
				Id = 1,
				Nome = "Cuiabá",
				Estado = "MT"
			};

			consultador.ExistemClientesVinculados(cidade)
				.Should()
				.BeTrue();
		}

	}
}
