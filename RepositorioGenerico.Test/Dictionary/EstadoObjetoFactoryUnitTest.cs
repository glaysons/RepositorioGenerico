using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Test.Dictionary
{
	[TestClass]
	public class EstadoObjetoFactoryUnitTest
	{
		[TestMethod]
		public void SeUmaListaCriarUmItemDeveGerarUmItemDoTipoCorretoJuntoComSeusFilhos()
		{
			var lista = new List<ObjetoDeTestes>();
			var item = lista.Criar();
			item
				.Should().NotBeNull();
			item
				.Should().BeOfType<ObjetoDeTestes>();
			item.Filhos
				.Should().NotBeNull();
		}
	}
}
