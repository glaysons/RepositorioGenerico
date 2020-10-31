using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Search;
using System.Data.SqlClient;
using RepositorioGenerico.Pattern.Buscadores;
using RepositorioGenerico.Search.Extensoes;
using FluentAssertions;
using RepositorioGenerico.Exceptions;

namespace RepositorioGenerico.Test.Search.Extensoes
{
	[TestClass]
	public class ConfiguradorExtensionUnitTest
	{

		private class ConfiguradorPersonalizado : IConfiguracao
		{
			public IConfiguracaoParametro DefinirParametro(string nome)
			{
				throw new NotImplementedException();
			}
		}

		[TestMethod]
		public void SeConfigurarScriptPersonalizadoParaUmConfiguradorDeveSerPossivelExecutar()
		{
			var comando = new SqlCommand();

			var config = new ConfiguradorQuery(comando, null);

			config.PersonalizarScript("hello world");
			config.Preparar();

			comando.CommandText
				.Should()
				.Be("hello world");
		}

		[TestMethod]
		public void SeConfigurarScriptPersonalizadoDeUmConfiguradorExternoNaoDevePermitirConfigurar()
		{
			var config = new ConfiguradorPersonalizado();

			Action personalizar = () => config.PersonalizarScript("hello world");

			personalizar
				.Should().Throw<EsteTipoDeConfiguracaoNaoPermitePersonalizacaoDeScript>();
		}

		[TestMethod]
		public void SeConfigurarScriptPersonalizadoDeUmObjetoNuloNaoDeveGerarErro()
		{
			Action personalizar = () => ((IConfiguracao)null).PersonalizarScript("hello world");

			personalizar
				.Should().NotThrow();
		}

	}
}
