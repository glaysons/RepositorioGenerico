using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositorioGenerico.Entities;
using RepositorioGenerico.Framework;
using RepositorioGenerico.Pattern.Contextos;

namespace RepositorioGenerico.Test.Framework
{
	[TestClass]
	public class ItemHistoricoTransacionalUnitTest
	{

		[TestMethod]
		public void AoCriarUmItemHistoricoTransacionalDeveAtribuirPropriedadesCorretamente()
		{
			var persistencia = Mock.Of<IPersistencia>();
			var registro = new Entidade();

			var item = new ItemHistoricoTransacional(persistencia, registro);

			item.Persistencia.Should().Be(persistencia);
			item.Registro.Should().Be(registro);
		}

	}
}
