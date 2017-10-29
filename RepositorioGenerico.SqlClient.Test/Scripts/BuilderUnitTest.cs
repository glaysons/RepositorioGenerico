using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositorioGenerico.Dictionary;
using RepositorioGenerico.Exceptions;
using RepositorioGenerico.SqlClient.Scripts;
using RepositorioGenerico.Test.Objetos;

namespace RepositorioGenerico.SqlClient.Test.Scripts
{
	[TestClass]
	public class BuilderUnitTest
	{
		[TestMethod]
		public void SeCriarScriptInsertDeObjetoComCampoIdentityDeveGerarScriptCorretamente()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			Builder.CriarScriptInsert(dicionario)
				.Should()
				.Be("insert into[ObjetoVirtual](" +
						"[CodigoNulo],[Nome],[Duplo],[DuploNulo],[Decimal]," +
						"[DecimalNulo],[Logico],[DataHora],[DataHoraNulo])" +
				    "values(" +
						"@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9) " +
				    "select @@identity");
		}

		[TestMethod]
		public void SeCriarScriptInsertDeObjetoComCampoSemIdentityDeveGerarScriptCorretamente()
		{
			var dicionario = new Dicionario(typeof(ObjetoComChaveComputada));

			Builder.CriarScriptInsert(dicionario)
				.Should()
				.Be("insert into[ObjetoComChaveComputada]([Codigo],[Nome])" +
				    "values(@p0,@p1)");
		}

		[TestMethod]
		public void SeCriarScriptUpdateDeveGerarScriptCorretamente()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			Builder.CriarScriptUpdate(dicionario)
				.Should()
				.Be("update[ObjetoVirtual]set" +
						"[CodigoNulo]=@p1,[Nome]=@p2,[Duplo]=@p3,[DuploNulo]=@p4,[Decimal]=@p5," +
						"[DecimalNulo]=@p6,[Logico]=@p7,[DataHora]=@p8,[DataHoraNulo]=@p9 " +
				    "where([Codigo]=@p0)");
		}

		[TestMethod]
		public void SeCriarScriptUpdateDeUmObjetoSemCamposChaveDeveGerarErro()
		{
			var dicionario = new Dicionario(typeof(ObjetoSemChaveDefinida));

			Action gerarScript = () => Builder.CriarScriptUpdate(dicionario);

			gerarScript
				.ShouldThrow<ChavePrimariaInvalidaException>();
		}

		[TestMethod]
		public void SeCriarScriptUpdateDeUmObjetoApenasComCamposChaveDeveGerarErro()
		{
			var dicionario = new Dicionario(typeof(ObjetoApenasComCamposChave));

			Action gerarScript = () => Builder.CriarScriptUpdate(dicionario);

			gerarScript
				.ShouldThrow<TabelaPossuiApenasCamposChavesException>();
		}

		[TestMethod]
		public void SeCriarScriptDeleteDeveGerarScriptCorretamente()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			Builder.CriarScriptDelete(dicionario)
				.Should()
				.Be("delete[ObjetoVirtual]" +
				    "where([Codigo]=@p0)");
		}

		[TestMethod]
		public void SeCriarScriptDeleteDeUmObjetoSemCamposChaveDeveGerarErro()
		{
			var dicionario = new Dicionario(typeof(ObjetoSemChaveDefinida));

			Action gerarScript = () => Builder.CriarScriptDelete(dicionario);

			gerarScript
				.ShouldThrow<ChavePrimariaInvalidaException>();
		}

		[TestMethod]
		public void SeCriarScriptAutoIncrementoDeObjetoComApenasUmCampoNaChaveDeveGerarCorretamente()
		{
			var dicionario = new Dicionario(typeof(ObjetoComChaveComputada));

			Builder.CriarScriptAutoIncremento(dicionario)
				.Should()
				.Be("select isnull((" +
						"select max([Codigo])" +
						"from[ObjetoComChaveComputada]" +
				    "),0)+1");
		}

		[TestMethod]
		public void SeCriarScriptAutoIncrementoDeObjetoComVariosCamposNaChaveDeveGerarCorretamente()
		{
			var dicionario = new Dicionario(typeof(ObjetoComChaveDuplaComputada));

			Builder.CriarScriptAutoIncremento(dicionario)
				.Should()
				.Be("select isnull((" +
						"select max([ChaveComputada])" +
						"from[ObjetoComChaveDuplaComputada]" +
						"where([ChaveBase]=@p0)" +
				    "),0)+1");
		}

		[TestMethod]
		public void SeCriarScriptAutoIncrementoDeUmObjetoSemChaveCalculadaDeveRetornarNulo()
		{
			var dicionario = new Dicionario(typeof(ObjetoDeTestes));

			Builder.CriarScriptAutoIncremento(dicionario)
				.Should()
				.BeNull();
		}

	}
}
