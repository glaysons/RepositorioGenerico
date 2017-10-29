using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Entities.Test
{
	[TestClass]
	public class ObjetoBancoUnitTest
	{

		[TestMethod]
		public void SeCriarUmObjetoDeBancoDeveSerIniciadoComoNovo()
		{
			new Entidade().EstadoEntidade
				.Should().Be(EstadosEntidade.Novo);
			new ObjetoDeTestes().EstadoEntidade
				.Should().Be(EstadosEntidade.Novo);
		}

		[TestMethod]
		public void SeClonarUmObjetoDeBancoDeveGerarCopiaComEstadoDeObjetoSempreNovo()
		{
			var funcionario = new ObjetoDeTestes()
			{
				Nome = "João",
				EstadoEntidade = EstadosEntidade.NaoModificado
			};

			funcionario.EstadoEntidade
				.Should().Be(EstadosEntidade.NaoModificado);

			var copia = funcionario.Clone() as ObjetoDeTestes;
			copia
				.Should().NotBeNull();

			if (copia == null)
				return;

			copia.EstadoEntidade
				.Should().Be(EstadosEntidade.Novo);
			copia.Nome
				.Should().Be("João");
		}

	}
}
