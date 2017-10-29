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

	}
}
