using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositorioGenerico.Fake.Test
{
	[TestClass]
	public class FabricaFakeUnitTest
	{

		[TestMethod]
		public void SeCriarUmContextoFakeNaoDeveGerarErro()
		{
			Action act = () => FabricaFake.CriarContexto();
			act.Should().NotThrow();
		}

		[TestMethod]
		public void SeCriarUmContextoLegadoFakeNaoDeveGerarErro()
		{
			Action act = () => FabricaFake.CriarContextoLegado();
			act.Should().NotThrow();
		}

		[TestMethod]
		public void SeCriarDoisContextoOsMesmosDevemSerDiferentes()
		{
			var contextoA = FabricaFake.CriarContexto();
			var contextoB = FabricaFake.CriarContexto();
			contextoA
				.Should()
				.NotBe(contextoB);
		}

		[TestMethod]
		public void SeCriarDoisContextoLegadosOsMesmosDevemSerDiferentes()
		{
			var contextoA = FabricaFake.CriarContextoLegado();
			var contextoB = FabricaFake.CriarContextoLegado();
			contextoA
				.Should()
				.NotBe(contextoB);
		}

	}
}
