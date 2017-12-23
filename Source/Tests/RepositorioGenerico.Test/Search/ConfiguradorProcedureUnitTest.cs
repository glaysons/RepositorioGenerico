using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Search;
using System.Data.SqlClient;
using FluentAssertions;

namespace RepositorioGenerico.Test.Search
{
	[TestClass]
	public class ConfiguradorProcedureUnitTest
	{

		[TestMethod]
		public void SeDefinirProcedureONomeDaProcedureDeveSerAtribuidoNoComando()
		{
			var comando = new SqlCommand();
			var config = new ConfiguradorProcedure(comando);

			config.DefinirProcedure("abc");

			config.Preparar();

			comando.CommandText
				.Should()
				.Be("abc");
		}

		[TestMethod]
		public void SePrepararOuPrepararExistenciaDoConfiguradorDeProcedureApenasONomeDaProcedureDeveSerAplicado()
		{
			var comando = new SqlCommand();
			var config = new ConfiguradorProcedure(comando);

			comando.CommandType = System.Data.CommandType.Text;
			comando.CommandText = string.Empty;

			config.DefinirProcedure("abc");

			config.Preparar();

			comando.CommandType
				.Should()
				.Be(System.Data.CommandType.StoredProcedure);

			comando.CommandText
				.Should()
				.Be("abc");

			comando.CommandType = System.Data.CommandType.Text;
			comando.CommandText = string.Empty;

			config.PrepararExistencia();

			comando.CommandType
				.Should()
				.Be(System.Data.CommandType.StoredProcedure);

			comando.CommandText
				.Should()
				.Be("abc");
		}
	}
}
