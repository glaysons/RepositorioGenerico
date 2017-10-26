using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.SqlClient.Scripts;
using RepositorioGenerico.Test;

namespace RepositorioGenerico.SqlClient.Test.Scripts
{
	[TestClass]
	public class CacheUnitTest
	{

		[TestMethod]
		public void SeConsultarVariasVezesOsMesmosScriptsDeveGerarUmaUnicaInstancia()
		{
			var dicionario = new Dicionario(typeof (ObjetoDeTestes));

			var script1 = Cache.Consultar(dicionario);
			var script2 = Cache.Consultar(dicionario);

			script1
				.Should()
				.BeSameAs(script2);
		}

	}
}
