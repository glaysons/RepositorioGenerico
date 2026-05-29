using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.Test.Dictionary
{
	[TestClass]
	public class DicionarioCacheUnitTest
	{

		[TestMethod]
		public void SeSolicitarDuasVezesOMesmoDicionarioDeveTrazerAMesmaInstancia()
		{

			var dicionario = DicionarioCache.Consultar(typeof(ObjetoDeTestes));
			DicionarioCache.Consultar(typeof(ObjetoDeTestes))
				.Should().Be(dicionario);

		}

		private static readonly object MeuLock = new object();

		[TestMethod]
		public void SeSolicitar100000VezesParalelasOMesmoDicionarioDeveSerOMesmo()
		{
			Dicionario dicionario = null;
			var vezes = 1;
			Parallel.For(1, 100000,
				n =>
				{
					if (dicionario == null)
						dicionario = DicionarioCache.Consultar(typeof(ObjetoDeTestes));
					DicionarioCache.Consultar(typeof(ObjetoDeTestes)).Should().Be(dicionario);
					lock (MeuLock)
						vezes++;
				});
			vezes.Should().Be(100000);
		}

		[TestMethod]
		public void SeConsultarDicionarioEmParaleloNaoDeveExporEstadoParcial()
		{
			var erros = new ConcurrentBag<Exception>();

			Parallel.For(0, 100000, _ =>
			{
				try
				{
					var dicionario = DicionarioCache.Consultar(typeof(ObjetoDeTestes));
					dicionario.QuantidadeCamposNaChave.Should().BeGreaterThan(0);
					dicionario.Itens.Should().NotBeEmpty();
				}
				catch (Exception ex)
				{
					erros.Add(ex);
				}
			});

			erros.Should().BeEmpty();
		}

	}
}
